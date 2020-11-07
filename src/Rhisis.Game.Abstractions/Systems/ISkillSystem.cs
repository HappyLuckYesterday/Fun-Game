using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Systems
{
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
    }
}
