using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Abstractions.Features.Battle
{
    public static class RangeAttackTypeExtensions
    {
        public static ObjectMessageType ToObjectMessageType(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.MeleeAttack1 => ObjectMessageType.OBJMSG_ATK1,
                AttackType.MeleeAttack2 => ObjectMessageType.OBJMSG_ATK2,
                AttackType.MeleeAttack3 => ObjectMessageType.OBJMSG_ATK3,
                AttackType.MeleeAttack4 => ObjectMessageType.OBJMSG_ATK4,
                AttackType.RangeBowAttack => ObjectMessageType.OBJMSG_ATK_RANGE1,
                AttackType.RangeWandAttack => ObjectMessageType.OBJMSG_ATK_MAGIC1,
                AttackType.SkillMeleeAttack => ObjectMessageType.OBJMSG_MELEESKILL,
                AttackType.SkillMagicAttack => ObjectMessageType.OBJMSG_MAGICSKILL,
                _ => throw new NotImplementedException($"the attack type of {attackType} can not be mapped to a object message type")
            };
        }

        public static bool IsMeleeAttack(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.MeleeAttack1 => true,
                AttackType.MeleeAttack2 => true,
                AttackType.MeleeAttack3 => true,
                AttackType.MeleeAttack4 => true,
                _ => false
            };
        }

        public static bool IsRangeAttack(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.RangeBowAttack => true,
                AttackType.RangeWandAttack => true,
                _ => false
            };
        }

        public static bool IsSkillAttack(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.SkillMeleeAttack => true,
                AttackType.SkillMagicAttack => true,
                _ => false
            };

        }

        public static bool CausesArrowProjectile(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.RangeBowAttack => true,
                _ => false
            };
        }

        public static bool CausesMagicProjectile(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.RangeWandAttack => true,
                _ => false
            };
        }

        public static bool CausesMeleeSkill(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.SkillMeleeAttack => true,
                _ => false
            };
        }

        public static bool CausesMagicSkill(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.SkillMagicAttack => true,
                _ => false
            };
        }

        public static AttackType ToAttackType(this SkillType skillType)
        {
            return skillType switch
            {
                SkillType.Magic => AttackType.SkillMagicAttack,
                SkillType.Skill => AttackType.SkillMeleeAttack,
                _ => throw new NotImplementedException($"there is no attack type for skill type {skillType}")
            };
        }
    }
}
