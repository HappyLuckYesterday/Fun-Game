using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the battle features.
    /// </summary>
    public interface IBattle
    {
        /// <summary>
        /// Gets a boolean value that indicates if the current mover is fighting.
        /// </summary>
        bool IsFighting { get; }

        /// <summary>
        /// Gets the battle target.
        /// </summary>
        IMover Target { get; set; }

        /// <summary>
        /// Attacks the given target if the target is valid.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attackType">Attack type.</param>
        /// <returns>true is the target was valid and the attack happened</returns>
        bool TryMeleeAttack(IMover target, AttackType attackType);

        /// <summary>
        /// Process a range attack of the given type (range or magic) on a given target if the target is valid.
        /// </summary>
        /// <param name="target">Target to attack.</param>
        /// <param name="power">Range attack power.</param>
        /// <param name="rangeAttackType">Range attack type.</param>
        /// <returns>true is the target was valid and the attack happened</returns>
        bool TryRangeAttack(IMover target, int power, AttackType rangeAttackType);

        /// <summary>
        /// Process a skill attack on a given target if the target is valid.
        /// </summary>
        /// <param name="target">Target to attack.</param>
        /// <param name="skill">Skill to execute.</param>
        /// <returns>true is the target was valid and the attack happened</returns>
        bool TrySkillAttack(IMover target, ISkill skill);

        /// <summary>
        /// Clears the battle target.
        /// </summary>
        void ClearTarget();
    }
}
