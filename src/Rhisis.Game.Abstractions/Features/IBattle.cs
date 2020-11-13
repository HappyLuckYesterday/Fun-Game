using Rhisis.Game.Abstractions.Entities;
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
        /// Checks if the current mover can attack the given mover.
        /// </summary>
        /// <param name="target">Target to attack.</param>
        /// <returns>True if the current mover can attack the mover; false otherwise.</returns>
        bool CanAttack(IMover target);

        /// <summary>
        /// Attacks the given target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="objectMessageType">Attack type.</param>
        void MeleeAttack(IMover target, ObjectMessageType objectMessageType);

        /// <summary>
        /// Process a range attack of the given type (range or magic) on a given target.
        /// </summary>
        /// <param name="target">Target to attack.</param>
        /// <param name="power">Range attack power.</param>
        /// <param name="objectMessageType">Range attack type.</param>
        /// <param name="projectileId">Projectile id.</param>
        void RangeAttack(IMover target, int power, ObjectMessageType objectMessageType, int projectileId);

        /// <summary>
        /// Process a skill attack on a given target.
        /// </summary>
        /// <param name="target">Target to attack.</param>
        /// <param name="skill">Skill to execute.</param>
        void SkillAttack(IMover target, ISkill skill);

        /// <summary>
        /// Clears the battle target.
        /// </summary>
        void ClearTarget();
    }
}
