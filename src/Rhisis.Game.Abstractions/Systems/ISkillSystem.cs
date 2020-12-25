using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechanism to manage player skills.
    /// </summary>
    public interface ISkillSystem
    {
        /// <summary>
        /// Use a skill.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="target">Target</param>
        /// <param name="skill">Skill to use.</param>
        /// <param name="skillUseType">Skill usage type.</param>
        void UseSkill(IPlayer player, IMover target, ISkill skill, SkillUseType skillUseType);

        /// <summary>
        /// Checks if the player can use the skill.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="target">Skill target.</param>
        /// <param name="skill">Skill to use.</param>
        /// <returns>True if the player can use the skill; false otherwise.</returns>
        bool CanUseSkill(IPlayer player, IMover target, ISkill skill);

        /// <summary>
        /// Applies a buff to the given target.
        /// </summary>
        /// <param name="target">Target to apply the buff.</param>
        /// <param name="buff">Buff to apply.</param>
        void ApplyBuff(IMover target, IBuff buff);
    }
}
