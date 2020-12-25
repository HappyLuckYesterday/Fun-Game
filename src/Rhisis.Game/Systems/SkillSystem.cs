using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Snapshots.Skills;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems
{
    [Injectable]
    internal class SkillSystem : GameFeature, ISkillSystem
    {
        private readonly ILogger<SkillSystem> _logger;
        private readonly IGameResources _gameResources;

        public SkillSystem(ILogger<SkillSystem> logger, IGameResources gameResources)
        {
            _logger = logger;
            _gameResources = gameResources;
        }

        public void UseSkill(IPlayer player, IMover target, ISkill skill, SkillUseType skillUseType)
        {
            switch (skill.Data.ExecuteTarget)
            {
                case SkillExecuteTargetType.MeleeAttack:
                    CastMeleeSkill(player, target, skill, skillUseType);
                    break;
                case SkillExecuteTargetType.MagicAttack:
                    CastMagicSkill(player, target, skill, skillUseType);
                    break;
                case SkillExecuteTargetType.MagicAttackShot:
                    CastMagicAttackShot(player, target, skill, skillUseType);
                    break;
                case SkillExecuteTargetType.AnotherWith:
                    CastBuffSkill(player, target, skill, skillUseType);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown {skill.Data.ExecuteTarget} for {skill.Name}");
            }
        }

        public bool CanUseSkill(IPlayer player, IMover target, ISkill skill)
        {
            if (skill.Level <= 0 || skill.Level > skill.Data.SkillLevels.Count)
            {
                return false;
            }

            if (!skill.IsCoolTimeElapsed())
            {
                SendDefinedText(player, DefineText.TID_GAME_SKILLWAITTIME);
                return false;
            }

            if (skill.LevelData.RequiredMP > 0 && player.Health.Mp < skill.LevelData.RequiredMP)
            {
                SendDefinedText(player, DefineText.TID_GAME_REQMP);
                return false;
            }

            if (skill.LevelData.RequiredFP > 0 && player.Health.Fp < skill.LevelData.RequiredFP)
            {
                SendDefinedText(player, DefineText.TID_GAME_REQFP);
                return false;
            }

            IItem rightWeapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

            if (skill.Data.LinkKind.HasValue)
            {
                bool playerHasCorrectWeapon;
                ItemKind3? rightWeaponKind = rightWeapon?.Data?.ItemKind3;

                switch (skill.Data.LinkKind)
                {
                    case ItemKind3.MAGICBOTH:
                        playerHasCorrectWeapon = !rightWeaponKind.HasValue ||
                            rightWeaponKind != ItemKind3.WAND || rightWeaponKind != ItemKind3.STAFF;
                        break;

                    case ItemKind3.YOBO:
                        playerHasCorrectWeapon = !rightWeaponKind.HasValue ||
                            rightWeaponKind != ItemKind3.YOYO || rightWeaponKind != ItemKind3.BOW;
                        break;

                    case ItemKind3.SHIELD:
                        IItem leftWeapon = player.Inventory.GetEquipedItem(ItemPartType.LeftWeapon);

                        playerHasCorrectWeapon = leftWeapon == null || leftWeapon.Data?.ItemKind3 != ItemKind3.SHIELD;
                        break;

                    default:
                        playerHasCorrectWeapon = skill.Data.LinkKind == rightWeaponKind;
                        break;
                }

                if (!playerHasCorrectWeapon)
                {
                    SendDefinedText(player, DefineText.TID_GAME_WRONGITEM);
                    return false;
                }
            }

            if (skill.Data.Type == SkillType.Magic)
            {
                BuffSkill buffSkill = target.Buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == skill.Id);

                if (buffSkill != null && buffSkill.SkillLevel > skill.Level)
                {
                    SendDefinedText(player, DefineText.TID_GAME_DONOTUSEBUFF);
                    return false;
                }
            }

            if (skill.Data.Handed.HasValue)
            {
                // TODO: handle dual weapons and two handed weapons
            }

            if (skill.Data.BulletLinkKind.HasValue)
            {
                IItem bulletItem = player.Inventory.GetEquipedItem(ItemPartType.Bullet);

                if (bulletItem.Id == -1 || bulletItem.Data.ItemKind2 != skill.Data.BulletLinkKind)
                {
                    DefineText errorText = skill.Data.LinkKind == ItemKind3.BOW ? DefineText.TID_TIP_NEEDSATTACKITEM : DefineText.TID_TIP_NEEDSKILLITEM;

                    SendDefinedText(player, errorText);
                    return false;
                }
            }

            // TODO: more skill checks

            return true;
        }

        public void ApplyBuff(IMover target, IBuff buff)
        {
            var buffState = target.Buffs.Add(buff);

            if (buffState != BuffResultType.None && buff is IBuffSkill buffSkill)
            {
                using var skillStateSnapshot = new SetSkillStateSnapshot(target, buffSkill.SkillId, buffSkill.SkillLevel, buff.RemainingTime);
                SendPacketToVisible(target, skillStateSnapshot, sendToPlayer: true);
            }
        }

        /// <summary>
        /// Casts a melee skill on a target.
        /// </summary>
        /// <param name="caster">Living entity casting a skill.</param>
        /// <param name="target">Living target entity touched by the skill.</param>
        /// <param name="skill">Skill to be casted.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastMeleeSkill(IMover caster, IMover target, ISkill skill, SkillUseType skillUseType)
        {
            var skillCastingTime = skill.GetCastingTime();

            if (skill.Data.SpellRegionType == SpellRegionType.Around)
            {
                throw new NotImplementedException("AoE skills");
            }
            else
            {
                using var snapshot = new UseSkillSnapshot(caster, target, skill, skillCastingTime, skillUseType);
                SendPacketToVisible(caster, snapshot, sendToPlayer: true);

                caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.ComboSkillTime), () =>
                {
                    ExecuteSkill(caster, target, skill);
                });
            }
        }

        /// <summary>
        /// Casts an instant magic skill on a living target entity.
        /// </summary>
        /// <param name="caster">Living entity casting a skill.</param>
        /// <param name="target">Living target entity touched by the skill.</param>
        /// <param name="skill">Skill to be casted.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastMagicSkill(IMover caster, IMover target, ISkill skill, SkillUseType skillUseType)
        {
            var skillCastingTime = skill.GetCastingTime();

            if (skill.Data.SpellRegionType == SpellRegionType.Around)
            {
                throw new NotImplementedException("AoE skills");
            }
            else
            {
                using var snapshot = new UseSkillSnapshot(caster, target, skill, skillCastingTime, skillUseType);
                SendPacketToVisible(caster, snapshot, sendToPlayer: true);

                caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.CastingTime), () =>
                {
                    ExecuteSkill(caster, target, skill);
                });
            }
        }

        /// <summary>
        /// Casts a magic attack shot projectile on a target.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="target">Target living entity.</param>
        /// <param name="skill">Skill to be casted as projectile.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastMagicAttackShot(IMover caster, IMover target, ISkill skill, SkillUseType skillUseType)
        {
            int skillCastingTime = skill.GetCastingTime();
            var projectile = new MagicSkillProjectile(caster, target, skill, () =>
            {
                ExecuteSkill(caster, target, skill, reduceCasterPoints: false);
            });
            caster.Projectiles.Add(projectile);

            using var snapshot = new UseSkillSnapshot(caster, target, skill, skillCastingTime, skillUseType);
            SendPacketToVisible(caster, snapshot, sendToPlayer: true);

            caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.CastingTime), () =>
            {
                ReduceCasterPoints(caster, skill);
            });
        }

        /// <summary>
        /// Executes a melee or magic skill and inflicts the damages to the target.
        /// </summary>
        /// <param name="caster">Living entity casting a skill.</param>
        /// <param name="target">Living target entity touched by the skill.</param>
        /// <param name="skill">Skill to be casted.</param>
        /// <param name="reduceCasterPoints">Value that indicates if it should reduce caster points or not.</param>
        private void ExecuteSkill(IMover caster, IMover target, ISkill skill, bool reduceCasterPoints = true)
        {
            IBattle battle = caster switch
            {
                IPlayer player => player.Battle,
                IMonster monster => monster.Battle,
                _ => null
            };

            if (battle == null)
            {
                throw new InvalidOperationException("Only a player or monster can cast a skill.");
            }

            if (battle.TrySkillAttack(target, skill))
            {
                skill.SetCoolTime(skill.LevelData.CooldownTime);

                if (reduceCasterPoints)
                {
                    ReduceCasterPoints(caster, skill);
                }
            }
        }

        /// <summary>
        /// Reduces caster fatigue points or mana points.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="skill">Casted skill by the living entity.</param>
        private void ReduceCasterPoints(IMover caster, ISkill skill)
        {
            using var updatePointsSnapshot = new FFSnapshot();

            if (skill.LevelData.RequiredFP > 0)
            {
                caster.Health.Fp -= skill.LevelData.RequiredFP;

                updatePointsSnapshot.Merge(new UpdateParamPointSnapshot(caster, DefineAttributes.FP, caster.Health.Fp));
            }

            if (skill.LevelData.RequiredMP > 0)
            {
                caster.Health.Mp -= skill.LevelData.RequiredMP;
                updatePointsSnapshot.Merge(new UpdateParamPointSnapshot(caster, DefineAttributes.MP, caster.Health.Mp));
            }

            if (updatePointsSnapshot.Count > 0)
            {
                SendPacketToVisible(caster, updatePointsSnapshot, sendToPlayer: true);
            }
        }

        /// <summary>
        /// Casts a buff skill on the given target.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="target">Target living entity.</param>
        /// <param name="skill">Skill to be casted as projectile.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastBuffSkill(IMover caster, IMover target, ISkill skill, SkillUseType skillUseType)
        {
            _logger.LogTrace($"{caster} is buffing {target} with skill {skill}");
            var castingTime = skill.GetCastingTime();

            if (target is IMonster)
            {
                using var cancelSkillSnapshot = new ClearUseSkillSnapshot(caster);

                SendPacketToVisible(caster, cancelSkillSnapshot, sendToPlayer: true);
                return;
            }

            if (skill.LevelData.DestParam1 > 0)
            {
                if (skill.LevelData.DestParam1 == DefineAttributes.HP)
                {
                    if (skill.LevelData.DestParam2 == DefineAttributes.RECOVERY_EXP)
                    {
                        // Resurection skill
                        // TODO: process resurection
                    }
                    else
                    {
                        _logger.LogTrace($"{caster} is healing {target} in {castingTime}...");
                        caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                        {
                            _logger.LogTrace($"{target} healed by {caster} !");
                            ApplyHealSkill(caster, target, skill);
                        });
                    }
                }
            }
            if (skill.LevelData.DestParam2 > 0)
            {
                if (skill.LevelData.DestParam2 == DefineAttributes.HP)
                {
                    caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                    {
                        ApplyHealSkill(caster, target, skill);
                    });
                }
            }

            var timeBonusValues = new int[]
            {
                skill.Data.ReferTarget1 == SkillReferTargetType.Time ? GetTimeBonus(caster, skill.Data.ReferStat1, skill.Data.ReferValue1, skill.Level) : default,
                skill.Data.ReferTarget2 == SkillReferTargetType.Time ? GetTimeBonus(caster, skill.Data.ReferStat2, skill.Data.ReferValue2, skill.Level) : default
            };

            var buffTime = skill.LevelData.SkillTime + timeBonusValues.Sum();

            if (buffTime > 0)
            {
                var attributes = new Dictionary<DefineAttributes, int>();

                if (skill.LevelData.DestParam1 > 0)
                {
                    attributes.Add(skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                }
                if (skill.LevelData.DestParam2 > 0)
                {
                    attributes.Add(skill.LevelData.DestParam2, skill.LevelData.DestParam2Value);
                }

                caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                {
                    var buff = new BuffSkill(target, attributes, skill.Data, skill.Level)
                    {
                        RemainingTime = buffTime
                    };

                    ApplyBuff(target, buff);
                });
            }

            skill.SetCoolTime(skill.LevelData.CooldownTime);

            using var snapshot = new UseSkillSnapshot(caster, target, skill, castingTime, skillUseType);

            SendPacketToVisible(caster, snapshot, sendToPlayer: true);
        }

        public int GetTimeBonus(IMover entity, DefineAttributes attribute, int value, int skillLevel)
            => GetReferBonus(entity, attribute, value, skillLevel);

        public int GetReferBonus(IMover entity, DefineAttributes attribute, int value, int skillLevel)
        {
            var attributeValue = attribute switch
            {
                DefineAttributes.STA => entity.Statistics.Stamina + entity.Attributes.Get(DefineAttributes.STA),
                DefineAttributes.DEX => entity.Statistics.Dexterity + entity.Attributes.Get(DefineAttributes.DEX),
                DefineAttributes.INT => entity.Statistics.Intelligence + entity.Attributes.Get(DefineAttributes.INT),
                _ => 1
            };

            return (int)(value / 10f * attributeValue + skillLevel * (attributeValue / 50f));
        }

        private void ApplyHealSkill(IMover caster, IMover target, ISkill skill)
        {
            if (skill.Data.ReferTarget1 == SkillReferTargetType.Heal || skill.Data.ReferTarget2 == SkillReferTargetType.Heal)
            {
                var hpValues = new int[]
                {
                    skill.Data.ReferTarget1 == SkillReferTargetType.Heal ? GetReferBonus(caster, skill.Data.ReferStat1, skill.Data.ReferValue1, skill.Level) : 0,
                    skill.Data.ReferTarget2 == SkillReferTargetType.Heal ? GetReferBonus(caster, skill.Data.ReferStat2, skill.Data.ReferValue2, skill.Level) : 0
                };

                var recoveredHp = skill.LevelData.DestParam1Value + hpValues.Sum();

                if (recoveredHp > 0)
                {
                    target.Health.Hp += recoveredHp;
                }
            }
        }
    }
}
