using Rhisis.Core.IO;
using Rhisis.Game.Battle;
using Rhisis.Game.Battle.AttackArbiters;
using Rhisis.Game.Battle.AttackArbiters.Reducers;
using Rhisis.Game.Battle.Projectiles;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Extensions;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;
using Rhisis.Game.Resources.Properties;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rhisis.Game;

[DebuggerDisplay("{Name} Lv.{Level}")]
public class Skill : IPacketSerializer
{
    private int _level;
    private long _nextSkillUsageTime;

    /// <summary>
    /// Gets the skill id.
    /// </summary>
    public int Id => Properties.Id;

    /// <summary>
    /// Gets the skill name.
    /// </summary>
    public string Name => Properties.Name;

    /// <summary>
    /// Gets the skill type.
    /// </summary>
    public SkillType Type => Properties.Type;

    /// <summary>
    /// Gets the skill owner instance.
    /// </summary>
    public Mover Owner { get; }

    /// <summary>
    /// Gets or sets the skill database id in case of the owner is a <see cref="Player"/>.
    /// </summary>
    public int? DatabaseId { get; set; }

    /// <summary>
    /// Gets or sets the skill level.
    /// </summary>
    public int Level
    {
        get => _level;
        set => _level = Math.Clamp(value, 0, Properties.MaxLevel);
    }

    /// <summary>
    /// Gets the skill properties.
    /// </summary>
    public SkillProperties Properties { get; }

    /// <summary>
    /// Gets the skill level properties.
    /// </summary>
    public SkillLevelProperties LevelProperties => Properties.SkillLevels[Level];

    public Skill(SkillProperties skillProperties, Mover owner, int level, int? databaseId = null)
    {
        Properties = skillProperties ?? throw new ArgumentNullException(nameof(skillProperties), "Cannot create a skill instance with undefined skill properties.");
        Owner = owner;
        Level = level;
        DatabaseId = databaseId;
    }

    /// <summary>
    /// Gets the skill casting in seconds.
    /// </summary>
    /// <returns>Skill casting time in seconds.</returns>
    public int GetCastingTime()
    {
        if (Properties.Type == SkillType.Skill)
        {
            return 1000;
        }
        else
        {
            int castingTime = (int)((LevelProperties.CastingTime / 1000f) * (60 / 4));

            castingTime -= castingTime * (Owner.Attributes.Get(DefineAttributes.DST_SPELL_RATE) / 100);

            return Math.Max(castingTime, 0);
        }
    }

    /// <summary>
    /// Sets the skill cool-time.
    /// </summary>
    /// <param name="coolTime">Skill cool time in milliseconds.</param>
    public void SetCoolTime(long coolTime)
    {
        if (coolTime > 0)
        {
            _nextSkillUsageTime = Time.GetElapsedTime() + coolTime;
        }
    }

    /// <summary>
    /// Gets a boolean value that indicates if the skill cool-time is elapsed.
    /// </summary>
    /// <returns>True if the cool-time is elapsed; false otherwise.</returns>
    public bool IsCoolTimeElapsed() => _nextSkillUsageTime < Time.GetElapsedTime();

    /// <summary>
    /// Serialize the skill into the given packet instance.
    /// </summary>
    /// <param name="packet">Packet.</param>
    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(Id);
        packet.WriteInt32(Level);
    }

    /// <summary>
    /// Compares the current instance with another <see cref="Skill"/> instance.
    /// </summary>
    /// <param name="otherSkill">Other skill instance.</param>
    /// <returns>True if the two skills are the same; false otherwise.</returns>
    public bool Equals([AllowNull] Skill otherSkill) => Id == otherSkill?.Id && Owner.ObjectId == otherSkill?.Owner.ObjectId;

    /// <summary>
    /// Checks if the current owner can use the current skill on the given target.
    /// </summary>
    /// <param name="target">Skill target.</param>
    /// <returns>True if the skill can be used; false otherwise.</returns>
    public bool CanUse(Mover target)
    {
        if (Level <= 0 || Level > Properties.SkillLevels.Count)
        {
            return false;
        }

        if (!IsCoolTimeElapsed())
        {
            Owner.SendDefinedText(DefineText.TID_GAME_SKILLWAITTIME);
            return false;
        }

        if (LevelProperties.RequiredMP > 0 && Owner.Health.Mp < LevelProperties.RequiredMP)
        {
            Owner.SendDefinedText(DefineText.TID_GAME_REQMP);
            return false;
        }

        if (LevelProperties.RequiredFP > 0 && Owner.Health.Fp < LevelProperties.RequiredFP)
        {
            Owner.SendDefinedText(DefineText.TID_GAME_REQFP);
            return false;
        }

        if (Owner is Player player)
        {
            if (Properties.LinkKind.HasValue)
            {
                Item rightWeapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);
                ItemKind3 rightWeaponKind = rightWeapon.Properties.ItemKind3;
                bool playerHasCorrectWeapon = Properties.LinkKind switch
                {
                    ItemKind3.MAGICBOTH => rightWeaponKind == ItemKind3.WAND || rightWeaponKind == ItemKind3.STAFF,
                    ItemKind3.YOBO => rightWeaponKind == ItemKind3.YOYO || rightWeaponKind == ItemKind3.BOW,
                    ItemKind3.SHIELD => player.Inventory.GetEquipedItem(ItemPartType.LeftWeapon).Properties.ItemKind3 != ItemKind3.SHIELD,
                    _ => Properties.LinkKind == rightWeaponKind,
                };

                if (!playerHasCorrectWeapon)
                {
                    Owner.SendDefinedText(DefineText.TID_GAME_WRONGITEM);
                    return false;
                }
            }

            if (Properties.BulletLinkKind.HasValue)
            {
                Item bulletItem = player.Inventory.GetEquipedItem(ItemPartType.Bullet);

                if (bulletItem.Properties.ItemKind2 != Properties.BulletLinkKind)
                {
                    DefineText errorText = Properties.LinkKind == ItemKind3.BOW ? 
                        DefineText.TID_TIP_NEEDSATTACKITEM : 
                        DefineText.TID_TIP_NEEDSKILLITEM;

                    Owner.SendDefinedText(errorText);
                    return false;
                }
            }
        }

        if (Type is SkillType.Magic)
        {
            // TODO: check buffs for target
        }

        if (Properties.Handed.HasValue)
        {
            // TODO: handle dual weapons and two handed weapons
        }

        return true;
    }

    /// <summary>
    /// Uses the current skill on the given target.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="skillUseType">Skill usage type.</param>
    public void Use(Mover target, SkillUseType skillUseType = SkillUseType.Normal)
    {
        switch (Properties.ExecuteTarget)
        {
            case SkillExecuteTargetType.MeleeAttack:
                CastMeleeSkill(target, skillUseType);
                break;
            case SkillExecuteTargetType.MagicAttack:
                CastMagicSkill(target, skillUseType);
                break;
            case SkillExecuteTargetType.MagicAttackShot:
                CastMagicAttackShot(target, skillUseType);
                break;
            case SkillExecuteTargetType.AnotherWith:
                CastBuffSkill(target, skillUseType);
                break;
            default:
                throw new InvalidOperationException($"Unknown {Properties.ExecuteTarget} for {Name}");
        }
    }

    private void CastMeleeSkill(Mover target, SkillUseType skillUseType)
    {
        var skillCastingTime = GetCastingTime();

        if (Properties.SpellRegionType == SpellRegionType.Around)
        {
            throw new NotImplementedException("AoE skills");
        }
        else
        {
            CastSkill(target, GetCastingTime(), LevelProperties.ComboSkillTime, skillUseType, () =>
            {
                Execute(target);
            });
        }
    }

    private void CastMagicSkill(Mover target, SkillUseType skillUseType)
    { 
        var skillCastingTime = GetCastingTime();

        if (Properties.SpellRegionType == SpellRegionType.Around)
        {
            throw new NotImplementedException("AoE skills");
        }
        else
        {
            CastSkill(target, skillCastingTime, LevelProperties.CastingTime, skillUseType, () =>
            {
                Execute(target);
            });
        }
    }

    private void CastMagicAttackShot(Mover target, SkillUseType skillUseType)
    {
        var skillCastingTime = GetCastingTime();
        MagicSkillProjectile projectile = new(Owner, target, this, () =>
        {
            Execute(target, reduceCasterPoints: false);
        });
        Owner.Projectiles.Add(projectile);

        CastSkill(target, skillCastingTime, LevelProperties.CastingTime, skillUseType, () =>
        {
            ReduceCasterPoints();
        });
    }

    private void CastBuffSkill(Mover target, SkillUseType skillUseType)
    {
        var skillCastingTime = GetCastingTime();

        if (target is not Player && Owner is Player player)
        {
            player.CancelSkillUsage();
            return;
        }

        if (LevelProperties.DestParam1 == DefineAttributes.DST_HP)
        {
            if (LevelProperties.DestParam2 == DefineAttributes.DST_RECOVERY_EXP)
            {
                // TODO: resurection
            }
            else
            {
                // TODO: heal
            }
        }

        if (LevelProperties.DestParam2 == DefineAttributes.DST_HP)
        {
            // TODO: heal
        }

        var timeBonusValues = new int[]
        {
            Properties.ReferTarget1 == SkillReferTargetType.Time ? GetReferBonus(Properties.ReferStat1, Properties.ReferValue1, Level) : default,
            Properties.ReferTarget2 == SkillReferTargetType.Time ? GetReferBonus(Properties.ReferStat2, Properties.ReferValue2, Level) : default
        };

        int buffTime = LevelProperties.SkillTime + timeBonusValues.Sum();

        if (buffTime > 0)
        {
            Dictionary<DefineAttributes, int> attributes = new();

            if (LevelProperties.DestParam1 > 0)
            {
                attributes.Add(LevelProperties.DestParam1, LevelProperties.DestParam1Value);
            }
            if (LevelProperties.DestParam2 > 0)
            {
                attributes.Add(LevelProperties.DestParam2, LevelProperties.DestParam2Value);
            }

            Owner.Delayer.DelayActionMilliseconds(GetCastingTime(), () =>
            {
                // TODO: apply buff
            });
        }

        SetCoolTime(LevelProperties.CooldownTime);
        SendSkillMotion(target, skillCastingTime, skillUseType);
    }

    private void CastSkill(Mover target, int skillCastingTime, int skillDelayTime, SkillUseType skillUseType, Action skillActionCallback)
    {
        if (skillActionCallback is null)
        {
            throw new ArgumentNullException(nameof(skillActionCallback));
        }

        SendSkillMotion(target, skillCastingTime, skillUseType);
        Owner.Delayer.DelayAction(TimeSpan.FromMilliseconds(skillDelayTime), () =>
        {
            skillActionCallback();
        });
    }

    private void Execute(Mover target, bool reduceCasterPoints = true)
    {
        if (!Owner.CanAttack(target))
        {
            return;
        }

        AttackType skillAttackType = Properties.Type.ToAttackType();

        if (!skillAttackType.IsSkillAttack())
        {
            return;
        }

        AttackResult attackResult = null;

        if (Owner is Player player && player.Mode.HasFlag(ModeType.ONEKILL_MODE))
        {
            attackResult = new AttackResult()
            {
                Damages = target.Health.Hp,
                Flags = AttackFlags.AF_GENERIC
            };
        }
        else
        {
            if (skillAttackType.CausesMeleeSkill())
            {
                attackResult = new MeleeSkillAttackArbiter(Owner, target, this).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MeleeSkillAttackReducer(Owner, target, this).ReduceDamages(attackResult);
                }
            }
            else if (skillAttackType.CausesMagicSkill())
            {
                attackResult = new MagicSkillAttackArbiter(Owner, target, this).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MagicSkillAttackReducer(Owner, target, this).ReduceDamages(attackResult);
                }
            }
        }

        if (attackResult is not null)
        {
            Owner.InflictDamages(target, attackResult, skillAttackType);
            SetCoolTime(LevelProperties.CooldownTime);

            if (reduceCasterPoints)
            {
                ReduceCasterPoints();
            }
        }
    }

    /// <summary>
    /// Reduces caster fatigue points or mana points.
    /// </summary>
    /// <param name="caster">Living entity casting the skill.</param>
    /// <param name="skill">Casted skill by the living entity.</param>
    private void ReduceCasterPoints()
    {
        using FFSnapshot updatePointsSnapshot = new();

        if (LevelProperties.RequiredFP > 0)
        {
            Owner.Health.Fp -= LevelProperties.RequiredFP;

            updatePointsSnapshot.Merge(new UpdateParamPointSnapshot(Owner, DefineAttributes.DST_FP, Owner.Health.Fp));
        }

        if (LevelProperties.RequiredMP > 0)
        {
            Owner.Health.Mp -= LevelProperties.RequiredMP;
            updatePointsSnapshot.Merge(new UpdateParamPointSnapshot(Owner, DefineAttributes.DST_MP, Owner.Health.Mp));
        }

        if (updatePointsSnapshot.Count > 0)
        {
            Owner.SendToVisible(updatePointsSnapshot, sendToSelf: true);
        }
    }

    private void SendSkillMotion(Mover target, int skillCastingTime, SkillUseType skillUseType)
    {
        using UseSkillSnapshot snapshot = new(Owner, target, this, skillCastingTime, skillUseType);

        Owner.SendToVisible(snapshot, sendToSelf: true);
    }

    private int GetReferBonus(DefineAttributes attribute, int value, int skillLevel)
    {
        var attributeValue = attribute switch
        {
            DefineAttributes.DST_STA => Owner.Statistics.Stamina + Owner.Attributes.Get(DefineAttributes.DST_STA),
            DefineAttributes.DST_DEX => Owner.Statistics.Dexterity + Owner.Attributes.Get(DefineAttributes.DST_DEX),
            DefineAttributes.DST_INT => Owner.Statistics.Intelligence + Owner.Attributes.Get(DefineAttributes.DST_INT),
            _ => 1
        };

        return (int)(value / 10f * attributeValue + skillLevel * (attributeValue / 50f));
    }

    public override string ToString() => Name;
}
