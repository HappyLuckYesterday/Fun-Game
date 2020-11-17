using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Abstractions.Features.Battle
{
    public static class RangeAttackTypeExtensions
    {
        /// <summary>
        /// The client uses the <see cref="ObjectMessageType"/> enum to indicate the attack type.
        /// This method will convert the <see cref="AttackType"/> known at the server to the attack type known by the client (<see cref="ObjectMessageType"/>).
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        /// <exception cref="NotImplementedException">The given attack type does not have a matching <see cref="ObjectMessageType"/>.</exception>
        /// <returns>The attack type as object message type.</returns>
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

        /// <summary>
        /// Checks if the given attack type causes a melee attack in combat or not.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
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

        /// <summary>
        /// Checks if the given attack type causes a ranged attack in combat or not.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        public static bool IsRangeAttack(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.RangeBowAttack => true,
                AttackType.RangeWandAttack => true,
                _ => false
            };
        }

        /// <summary>
        /// Checks if the given attack type causes a skill attack in combat or not.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        public static bool IsSkillAttack(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.SkillMeleeAttack => true,
                AttackType.SkillMagicAttack => true,
                _ => false
            };
        }

        /// <summary>
        /// Checks if the given attack type should cause a arrow projectile to be launched during combat.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        public static bool CausesArrowProjectile(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.RangeBowAttack => true,
                _ => false
            };
        }

        /// <summary>
        /// Checks if the given attack type should cause a magic projectile to be launched during combat.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        public static bool CausesMagicProjectile(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.RangeWandAttack => true,
                _ => false
            };
        }

        /// <summary>
        /// Checks if the given attack type should cause a melee skill to be cast during combat.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        public static bool CausesMeleeSkill(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.SkillMeleeAttack => true,
                _ => false
            };
        }

        /// <summary>
        /// Checks if the given attack type should cause a magic skill to be cast during combat.
        /// </summary>
        /// <param name="attackType">The attack type.</param>
        public static bool CausesMagicSkill(this AttackType attackType)
        {
            return attackType switch
            {
                AttackType.SkillMagicAttack => true,
                _ => false
            };
        }

        /// <summary>
        /// Decides which type of attack should be used when a skill of the given type is used in combat.
        /// </summary>
        /// <param name="skillType">The skill type.</param>
        /// <exception cref="NotImplementedException">The given skill type does not have a matching attack type.</exception>
        /// <returns>An attack type that matches the given skill type.</returns>
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
