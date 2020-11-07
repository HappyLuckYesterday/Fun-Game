using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
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

            //if (skill.Data.Type == SkillType.Magic)
            //{
            //    BuffSkill buffSkill = target.Buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == skill.SkillId);

            //    if (buffSkill != null && buffSkill.SkillLevel > skill.Level)
            //    {
            //        _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_DONOTUSEBUFF);
            //        return false;
            //    }
            //}

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
                // TODO: AoE
                throw new NotImplementedException("AoE skills");
            }
            else
            {
                //_skillPacketFactory.SendUseSkill(caster, target, skill, skillCastingTime, skillUseType);

                //caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.ComboSkillTime), () =>
                //{
                //    ExecuteSkill(caster, target, skill);
                //});
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
                // TODO: AoE
                throw new NotImplementedException("AoE skills");
            }
            else
            {
                //_skillPacketFactory.SendUseSkill(caster, target, skill, skillCastingTime, skillUseType);

                //caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.CastingTime), () =>
                //{
                //    ExecuteSkill(caster, target, skill);
                //});
            }
        }

        /// <summary>
        /// Executes a melee or magic skill and inflicts the damages to the target.
        /// </summary>
        /// <param name="caster">Living entity casting a skill.</param>
        /// <param name="target">Living target entity touched by the skill.</param>
        /// <param name="skill">Skill to be casted.</param>
        /// <param name="reduceCasterPoints">Value that indicates if it should reduce caster points or not.</param>
        private void ExecuteSkill(IMover caster, IMover target, Skill skill, bool reduceCasterPoints = true)
        {
            //ObjectMessageType skillMessageType = ObjectMessageType.OBJMSG_NONE;
            //IAttackArbiter attackArbiter = null;

            //switch (skill.Data.Type)
            //{
            //    case SkillType.Magic:
            //        attackArbiter = new MagicSkillAttackArbiter(caster, target, skill);
            //        skillMessageType = ObjectMessageType.OBJMSG_MAGICSKILL;
            //        break;

            //    case SkillType.Skill:
            //        attackArbiter = new MeleeSkillAttackArbiter(caster, target, skill);
            //        skillMessageType = ObjectMessageType.OBJMSG_MELEESKILL;
            //        break;
            //}

            //_battleSystem.DamageTarget(caster, target, attackArbiter, skillMessageType);

            //skill.SetCoolTime(skill.LevelData.CooldownTime);

            //if (reduceCasterPoints)
            //{
            //    ReduceCasterPoints(caster, skill);
            //}
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
            //var skillCastingTime = skill.GetCastingTime();
            //var projectile = new MagicSkillProjectileInfo(caster, target, skill, () =>
            //{
            //    ExecuteSkill(caster, target, skill, reduceCasterPoints: false);
            //});

            //_skillPacketFactory.SendUseSkill(caster, target, skill, skillCastingTime, skillUseType);

            //caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.CastingTime), () =>
            //{
            //    ReduceCasterPoints(caster, skill);
            //});
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
                //_skillPacketFactory.SendSkillCancellation(caster as IPlayerEntity);
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
                        //caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                        //{
                        //    _logger.LogTrace($"{target} healed by {caster} !");
                        //    ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                        //});
                    }
                }
            }
            if (skill.LevelData.DestParam2 > 0)
            {
                if (skill.LevelData.DestParam2 == DefineAttributes.HP)
                {
                    //caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                    //{
                    //    ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                    //});
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

                //caster.Delayer.DelayActionMilliseconds(castingTime, () =>
                //{
                //    var buff = new BuffSkill(skill.SkillId, skill.Level, buffTime, attributes);
                //    bool isBuffAdded = _buffSystem.AddBuff(target, buff);

                //    if (isBuffAdded)
                //    {
                //        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                //        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam2, skill.LevelData.DestParam2Value);

                //        _skillPacketFactory.SendSkillState(target, buff.SkillId, buff.SkillLevel, buff.RemainingTime);
                //    }
                //});
            }

            skill.SetCoolTime(skill.LevelData.CooldownTime);

            //_skillPacketFactory.SendUseSkill(caster, target, skill, castingTime, skillUseType);
        }

        public int GetTimeBonus(IMover entity, DefineAttributes attribute, int value, int skillLevel)
            => GetReferBonus(entity, attribute, value, skillLevel);

        public int GetReferBonus(IMover entity, DefineAttributes attribute, int value, int skillLevel)
        {
            var attributeValue = attribute switch
            {
                //DefineAttributes.STA => _statisticsSystem.GetTotalStrength(entity),
                //DefineAttributes.DEX => _statisticsSystem.GetTotalDexterity(entity),
                //DefineAttributes.INT => _statisticsSystem.GetTotalIntelligence(entity),
                _ => 1
            };

            return (int)(value / 10f * attributeValue + skillLevel * (attributeValue / 50f));
        }

        private void ApplySkillParameters(IMover caster, IMover target, ISkill skill, DefineAttributes attribute, int value)
        {
            switch (attribute)
            {
                case DefineAttributes.HP:
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
                            //_attributeSystem.SetAttribute(target, attribute, recoveredHp);
                        }
                    }
                    break;
                default:
                    //_attributeSystem.SetAttribute(target, attribute, value);
                    break;
            }
        }


    }
}
