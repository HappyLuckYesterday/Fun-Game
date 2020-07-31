using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Attributes;
using Rhisis.World.Systems.Battle;
using Rhisis.World.Systems.Battle.Arbiters;
using Rhisis.World.Systems.Buff;
using Rhisis.World.Systems.Health;
using Rhisis.World.Systems.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Skills
{
    [Injectable]
    internal class SkillSystem : ISkillSystem
    {
        /// <summary>
        /// Skill points usage based on the job type.
        /// </summary>
        private static readonly IReadOnlyDictionary<DefineJob.JobType, int> SkillPointUsage = new Dictionary<DefineJob.JobType, int>
        {
            { DefineJob.JobType.JTYPE_BASE, 1 },
            { DefineJob.JobType.JTYPE_EXPERT, 2 },
            { DefineJob.JobType.JTYPE_PRO, 3 },
            { DefineJob.JobType.JTYPE_MASTER, 3 },
            { DefineJob.JobType.JTYPE_HERO, 3 }
        };

        private readonly ILogger<SkillSystem> _logger;
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly IBattleSystem _battleSystem;
        private readonly IAttributeSystem _attributeSystem;
        private readonly IBuffSystem _buffSystem;
        private readonly IStatisticsSystem _statisticsSystem;
        private readonly ISkillPacketFactory _skillPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;
        private readonly IPlayerPacketFactory _playerPacketFactory;
        private readonly ISpecialEffectPacketFactory _specialEffectPacketFactory;
        private readonly IMoverPacketFactory _moverPacketFactory;

        /// <inheritdoc />
        public int Order => 1;

        public SkillSystem(ILogger<SkillSystem> logger, IRhisisDatabase database, IGameResources gameResources,
            IBattleSystem battleSystem, IAttributeSystem attributeSystem, IBuffSystem buffSystem, IStatisticsSystem statisticsSystem,
            ISkillPacketFactory skillPacketFactory, ITextPacketFactory textPacketFactory, IPlayerPacketFactory playerPacketFactory,
            ISpecialEffectPacketFactory specialEffectPacketFactory, IMoverPacketFactory moverPacketFactory)
        {
            _logger = logger;
            _database = database;
            _gameResources = gameResources;
            _battleSystem = battleSystem;
            _attributeSystem = attributeSystem;
            _buffSystem = buffSystem;
            _statisticsSystem = statisticsSystem;
            _skillPacketFactory = skillPacketFactory;
            _textPacketFactory = textPacketFactory;
            _playerPacketFactory = playerPacketFactory;
            _specialEffectPacketFactory = specialEffectPacketFactory;
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<Skill> jobSkills = GetSkillsByJob(player.PlayerData.Job);
            IEnumerable<DbSkill> playerSkills = _database.Skills.Where(x => x.CharacterId == player.PlayerData.Id).AsNoTracking().AsEnumerable();

            player.SkillTree.Skills = (from x in jobSkills
                                       join s in playerSkills on x.SkillId equals s.SkillId into dbSkills
                                       from dbSkill in dbSkills.DefaultIfEmpty()
                                       select new Skill(x.SkillId, player.PlayerData.Id, _gameResources.Skills[x.SkillId], dbSkill?.Level ?? default, dbSkill?.Id)).ToList();
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            var skillsSet = from x in _database.Skills.Where(x => x.CharacterId == player.PlayerData.Id).AsNoTracking().ToList()
                            join s in player.SkillTree.Skills on
                             new { x.SkillId, x.CharacterId }
                             equals
                             new { s.SkillId, s.CharacterId }
                            select new { DbSkill = x, PlayerSkill = s };

            foreach (var skillToUpdate in skillsSet)
            {
                skillToUpdate.DbSkill.Level = (byte)skillToUpdate.PlayerSkill.Level;

                _database.Skills.Update(skillToUpdate.DbSkill);
            }

            foreach (Skill skill in player.SkillTree.Skills)
            {
                if (!skill.DatabaseId.HasValue && skill.Level > 0)
                {
                    var newSkill = new DbSkill
                    {
                        SkillId = skill.SkillId,
                        Level = (byte)skill.Level,
                        CharacterId = player.PlayerData.Id
                    };

                    _database.Skills.Add(newSkill);
                }
            }

            _database.SaveChanges();
        }

        public IEnumerable<Skill> GetSkillsByJob(DefineJob.Job job)
        {
            var skillsList = new List<Skill>();

            if (_gameResources.Jobs.TryGetValue(job, out JobData jobData) && jobData.Parent != null)
            {
                skillsList.AddRange(GetSkillsByJob(jobData.Parent.Id));
            }

            IEnumerable<Skill> jobSkills = from x in _gameResources.Skills.Values
                                           where x.Job == jobData.Id &&
                                                 x.JobType != DefineJob.JobType.JTYPE_COMMON &&
                                                 x.JobType != DefineJob.JobType.JTYPE_TROUPE
                                           select new Skill(x.Id, -1, x);

            skillsList.AddRange(jobSkills);

            return skillsList;
        }

        /// <inheritdoc />
        public void UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            int requiredSkillPoints = 0;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;
                Skill playerSkill = player.SkillTree.GetSkill(skillId);

                if (playerSkill == null)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot find skill with id '{skillId}' for player '{player}'.");
                    return;
                }

                if (playerSkill.Level == skillLevel)
                {
                    // Skill hasn't change
                    continue;
                }

                if (player.Object.Level < playerSkill.Data.RequiredLevel)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Player need to be level '{playerSkill.Data.RequiredLevel}' to learn this skill.");
                    return;
                }

                if (!CheckRequiredSkill(playerSkill.Data.RequiredSkillId1, playerSkill.Data.RequiredSkillLevel1, skillsToUpdate))
                {
                    SkillData requiredSkill1 = _gameResources.Skills[playerSkill.Data.RequiredSkillId1];

                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Skill '{requiredSkill1.Name}' must be at least Lv.{requiredSkill1.RequiredSkillLevel1}");
                    return;
                }

                if (!CheckRequiredSkill(playerSkill.Data.RequiredSkillId2, playerSkill.Data.RequiredSkillLevel2, skillsToUpdate))
                {
                    SkillData requiredSkill2 = _gameResources.Skills[playerSkill.Data.RequiredSkillId2];

                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Skill '{requiredSkill2.Name}' must be at least Lv.{requiredSkill2.RequiredSkillLevel1}");
                    return;
                }

                if (skillLevel < 0 || skillLevel < playerSkill.Level || skillLevel > playerSkill.Data.MaxLevel)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. The skill level is out of bounds.");
                    return;
                }

                if (!SkillPointUsage.TryGetValue(playerSkill.Data.JobType, out int requiredSkillPointAmount))
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Cannot find required skill point for job type '{playerSkill.Data.JobType}'.");
                    return;
                }

                requiredSkillPoints += (skillLevel - playerSkill.Level) * requiredSkillPointAmount;
            }

            if (player.Statistics.SkillPoints < requiredSkillPoints)
            {
                _logger.LogError($"Cannot update skills for player '{player}'. Not enough skill points.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                return;
            }

            player.Statistics.SkillPoints -= (ushort)requiredSkillPoints;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;

                player.SkillTree.SetSkillLevel(skillId, skillLevel);
            }

            _specialEffectPacketFactory.SendSpecialEffect(player, DefineSpecialEffects.XI_SYS_EXCHAN01, false);
            _skillPacketFactory.SendSkillTreeUpdate(player);
        }

        /// <inheritdoc />
        public void UseSkill(IPlayerEntity player, Skill skill, uint targetObjectId, SkillUseType skillUseType)
        {
            if (skill == null)
            {
                _skillPacketFactory.SendSkillCancellation(player);
                throw new ArgumentNullException(nameof(skill), $"Cannot use undefined skill for player {player} on target {targetObjectId}.");
            }

            ILivingEntity target;

            if (player.Id == targetObjectId)
            {
                target = player;
            }
            else
            {
                target = player.FindEntity<ILivingEntity>(targetObjectId);

                if (target == null)
                {
                    _skillPacketFactory.SendSkillCancellation(player);
                    throw new ArgumentNullException(nameof(targetObjectId));
                }
            }

            if (CanUseSkill(player, target, skill))
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
                        _logger.LogDebug($"Unknown {skill.Data.ExecuteTarget} for {skill.Name}");
                        _skillPacketFactory.SendSkillCancellation(player);
                        break;
                }
            }
            else
            {
                _logger.LogDebug($"Cannot use {skill.Data.Name}.");
                _skillPacketFactory.SendSkillCancellation(player);
            }
        }

        /// <inheritdoc />
        public bool CanUseSkill(IPlayerEntity player, ILivingEntity target, Skill skill)
        {
            if (skill.Level <= 0 || skill.Level > skill.Data.SkillLevels.Count)
            {
                return false;
            }

            if (!skill.IsCoolTimeElapsed())
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_SKILLWAITTIME);
                return false;
            }

            if (skill.LevelData.RequiredMP > 0 && player.Attributes[DefineAttributes.MP] < skill.LevelData.RequiredMP)
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQMP);
                return false;
            }

            if (skill.LevelData.RequiredFP > 0 && player.Attributes[DefineAttributes.FP] < skill.LevelData.RequiredFP)
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQFP);
                return false;
            }

            Item rightWeapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

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
                        Item leftWeapon = player.Inventory.GetEquipedItem(ItemPartType.LeftWeapon);

                        playerHasCorrectWeapon = leftWeapon == null || leftWeapon.Data?.ItemKind3 != ItemKind3.SHIELD;
                        break;

                    default:
                        playerHasCorrectWeapon = skill.Data.LinkKind == rightWeaponKind;
                        break;
                }

                if (!playerHasCorrectWeapon)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_WRONGITEM);
                    return false;
                }
            }

            if (skill.Data.Type == SkillType.Magic)
            {
                BuffSkill buffSkill = target.Buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == skill.SkillId);

                if (buffSkill != null && buffSkill.SkillLevel > skill.Level)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_DONOTUSEBUFF);
                    return false;
                }
            }

            if (skill.Data.Handed.HasValue)
            {
                // TODO: handle dual weapons and two handed weapons
            }

            if (skill.Data.BulletLinkKind.HasValue)
            {
                Item bulletItem = player.Inventory.GetEquipedItem(ItemPartType.Bullet);

                if (bulletItem.Id == -1 || bulletItem.Data.ItemKind2 != skill.Data.BulletLinkKind)
                {
                    DefineText errorText = skill.Data.LinkKind == ItemKind3.BOW ? DefineText.TID_TIP_NEEDSATTACKITEM : DefineText.TID_TIP_NEEDSKILLITEM;

                    _textPacketFactory.SendDefinedText(player, errorText);
                    return false;
                }
            }

            // TODO: more skill checks

            return true;
        }

        public void Reskill(IPlayerEntity player)
        {
            foreach (Skill skill in player.SkillTree.Skills)
            {
                player.Statistics.SkillPoints += (ushort)(skill.Level * SkillPointUsage[skill.Data.JobType]);
                skill.Level = 0;
            }

            _skillPacketFactory.SendSkillReset(player, player.Statistics.SkillPoints);
        }

        public void AddSkillPoints(IPlayerEntity player, ushort skillPoints)
        {
            if (skillPoints > 0)
            {
                player.Statistics.SkillPoints += skillPoints;
                _playerPacketFactory.SendPlayerExperience(player);
            }
        }

        /// <summary>
        /// Check if the required skill condition matches.
        /// </summary>
        /// <param name="requiredSkillId">Required skill id.</param>
        /// <param name="requiredSkillLevel">Required skill level.</param>
        /// <param name="skillsToUpdate">Dictionary of skills to update.</param>
        /// <returns>True if the requirement matches; false otherwise.</returns>
        private bool CheckRequiredSkill(int requiredSkillId, int requiredSkillLevel, IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            if (requiredSkillId == -1)
            {
                return true;
            }

            if (skillsToUpdate.TryGetValue(requiredSkillId, out int skillLevel))
            {
                return skillLevel >= requiredSkillLevel;
            }

            return false;
        }

        /// <summary>
        /// Gets the skill casting time.
        /// </summary>
        /// <param name="caster">Entity casting the skill.</param>
        /// <param name="skill">Skill to be cast.</param>
        /// <returns>Skill casting time in milliseconds.</returns>
        private int GetSkillCastingTime(ILivingEntity caster, Skill skill)
        {
            if (skill.Data.Type == SkillType.Skill)
            {
                return 1 * 1000;
            }
            else
            {
                int castingTime = (int)((skill.LevelData.CastingTime / 1000f) * (60 / 4));

                castingTime -= castingTime * (caster.Attributes[DefineAttributes.SPELL_RATE] / 100);

                return Math.Max(castingTime, 0);
            }
        }

        /// <summary>
        /// Casts a melee skill on a target.
        /// </summary>
        /// <param name="caster">Living entity casting a skill.</param>
        /// <param name="target">Living target entity touched by the skill.</param>
        /// <param name="skill">Skill to be casted.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastMeleeSkill(ILivingEntity caster, ILivingEntity target, Skill skill, SkillUseType skillUseType)
        {
            int skillCastingTime = GetSkillCastingTime(caster, skill);

            if (skill.Data.SpellRegionType == SpellRegionType.Around)
            {
                // TODO: AoE
                _textPacketFactory.SendFeatureNotImplemented(caster as IPlayerEntity, "AoE skills");
            }
            else
            {
                _skillPacketFactory.SendUseSkill(caster, target, skill, skillCastingTime, skillUseType);

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
        private void CastMagicSkill(ILivingEntity caster, ILivingEntity target, Skill skill, SkillUseType skillUseType)
        {
            int skillCastingTime = GetSkillCastingTime(caster, skill);

            if (skill.Data.SpellRegionType == SpellRegionType.Around)
            {
                // TODO: AoE
                _textPacketFactory.SendFeatureNotImplemented(caster as IPlayerEntity, "AoE skills");
            }
            else
            {
                _skillPacketFactory.SendUseSkill(caster, target, skill, skillCastingTime, skillUseType);

                caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.CastingTime), () =>
                {
                    ExecuteSkill(caster, target, skill);
                });
            }
        }

        /// <summary>
        /// Executes a melee or magic skill and inflicts the damages to the target.
        /// </summary>
        /// <param name="caster">Living entity casting a skill.</param>
        /// <param name="target">Living target entity touched by the skill.</param>
        /// <param name="skill">Skill to be casted.</param>
        /// <param name="reduceCasterPoints">Value that indicates if it should reduce caster points or not.</param>
        private void ExecuteSkill(ILivingEntity caster, ILivingEntity target, Skill skill, bool reduceCasterPoints = true)
        {
            ObjectMessageType skillMessageType = ObjectMessageType.OBJMSG_NONE;
            IAttackArbiter attackArbiter = null;

            switch (skill.Data.Type)
            {
                case SkillType.Magic:
                    attackArbiter = new MagicSkillAttackArbiter(caster, target, skill);
                    skillMessageType = ObjectMessageType.OBJMSG_MAGICSKILL;
                    break;

                case SkillType.Skill:
                    attackArbiter = new MeleeSkillAttackArbiter(caster, target, skill);
                    skillMessageType = ObjectMessageType.OBJMSG_MELEESKILL;
                    break;
            }

            _battleSystem.DamageTarget(caster, target, attackArbiter, skillMessageType);

            skill.SetCoolTime(skill.LevelData.CooldownTime);

            if (reduceCasterPoints)
            {
                ReduceCasterPoints(caster, skill);
            }
        }

        /// <summary>
        /// Casts a magic attack shot projectile on a target.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="target">Target living entity.</param>
        /// <param name="skill">Skill to be casted as projectile.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastMagicAttackShot(ILivingEntity caster, ILivingEntity target, Skill skill, SkillUseType skillUseType)
        {
            int skillCastingTime = GetSkillCastingTime(caster, skill);
            var projectile = new MagicSkillProjectileInfo(caster, target, skill, () =>
            {
                ExecuteSkill(caster, target, skill, reduceCasterPoints: false);
            });

            //_projectileSystem.CreateProjectile(projectile);
            _skillPacketFactory.SendUseSkill(caster, target, skill, skillCastingTime, skillUseType);

            caster.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.CastingTime), () =>
            {
                ReduceCasterPoints(caster, skill);
            });
        }

        /// <summary>
        /// Reduces caster fatigue points or mana points.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="skill">Casted skill by the living entity.</param>
        private void ReduceCasterPoints(ILivingEntity caster, Skill skill)
        {
            if (skill.LevelData.RequiredFP > 0)
            {
                caster.Attributes[DefineAttributes.FP] -= skill.LevelData.RequiredFP;
                _moverPacketFactory.SendUpdatePoints(caster, DefineAttributes.FP, caster.Attributes[DefineAttributes.FP]);
            }

            if (skill.LevelData.RequiredMP > 0)
            {
                caster.Attributes[DefineAttributes.MP] -= skill.LevelData.RequiredMP;
                _moverPacketFactory.SendUpdatePoints(caster, DefineAttributes.MP, caster.Attributes[DefineAttributes.MP]);
            }
        }

        /// <summary>
        /// Casts a buff skill on the given target.
        /// </summary>
        /// <param name="caster">Living entity casting the skill.</param>
        /// <param name="target">Target living entity.</param>
        /// <param name="skill">Skill to be casted as projectile.</param>
        /// <param name="skillUseType">Skill use type.</param>
        private void CastBuffSkill(ILivingEntity caster, ILivingEntity target, Skill skill, SkillUseType skillUseType)
        {
            _logger.LogTrace($"{caster} is buffing {target} with skill {skill}");
            int castingTime = GetSkillCastingTime(caster, skill);

            if (target.Type == WorldEntityType.Monster)
            {
                _skillPacketFactory.SendSkillCancellation(caster as IPlayerEntity);
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
                        caster.Delayer.DelayAction(castingTime / 1000f, () =>
                        {
                            _logger.LogTrace($"{target} healed by {caster} !");
                            ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                        });
                    }
                }
            }
            if (skill.LevelData.DestParam2 > 0)
            {
                if (skill.LevelData.DestParam2 == DefineAttributes.HP)
                {
                    caster.Delayer.DelayAction(castingTime / 1000f, () =>
                    {
                        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                    });
                }
            }

            var timeBonusValues = new int[]
            {
                skill.Data.ReferTarget1 == SkillReferTargetType.Time ? GetTimeBonus(caster, skill.Data.ReferStat1, skill.Data.ReferValue1, skill.Level) : default,
                skill.Data.ReferTarget2 == SkillReferTargetType.Time ? GetTimeBonus(caster, skill.Data.ReferStat2, skill.Data.ReferValue2, skill.Level) : default
            };

            int buffTime = skill.LevelData.SkillTime + timeBonusValues.Sum();

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

                caster.Delayer.DelayAction(castingTime / 1000f, () =>
                {
                    var buff = new BuffSkill(skill.SkillId, skill.Level, buffTime, attributes);
                    bool isBuffAdded = _buffSystem.AddBuff(target, buff);

                    if (isBuffAdded)
                    {
                        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam1, skill.LevelData.DestParam1Value);
                        ApplySkillParameters(caster, target, skill, skill.LevelData.DestParam2, skill.LevelData.DestParam2Value);

                        _skillPacketFactory.SendSkillState(target, buff.SkillId, buff.SkillLevel, buff.RemainingTime);
                    }
                });
            }

            skill.SetCoolTime(skill.LevelData.CooldownTime);

            _skillPacketFactory.SendUseSkill(caster, target, skill, castingTime, skillUseType);
        }

        public int GetTimeBonus(ILivingEntity entity, DefineAttributes attribute, int value, int skillLevel)
            => GetReferBonus(entity, attribute, value, skillLevel);

        public int GetReferBonus(ILivingEntity entity, DefineAttributes attribute, int value, int skillLevel)
        {
            int attributeValue = attribute switch
            {
                DefineAttributes.STA => _statisticsSystem.GetTotalStrength(entity),
                DefineAttributes.DEX => _statisticsSystem.GetTotalDexterity(entity),
                DefineAttributes.INT => _statisticsSystem.GetTotalIntelligence(entity),
                _ => 1
            };

            return (int)(((value / 10f) * attributeValue) + (skillLevel * (attributeValue / 50f)));
        }

        private void ApplySkillParameters(ILivingEntity caster, ILivingEntity target, Skill skill, DefineAttributes attribute, int value)
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

                        int recoveredHp = skill.LevelData.DestParam1Value + hpValues.Sum();

                        if (recoveredHp > 0)
                        {
                            _attributeSystem.SetAttribute(target, attribute, recoveredHp);
                        }
                    }
                    break;
                default:
                    _attributeSystem.SetAttribute(target, attribute, value);
                    break;
            }
        }


    }
}
