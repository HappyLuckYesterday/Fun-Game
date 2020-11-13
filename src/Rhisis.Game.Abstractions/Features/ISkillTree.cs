using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;
using System;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the player skill tree.
    /// </summary>
    public interface ISkillTree : IPacketSerializer, IEnumerable<ISkill>
    {
        /// <summary>
        /// Gets the available skill points.
        /// </summary>
        ushort SkillPoints { get; }

        /// <summary>
        /// Adds skill points.
        /// </summary>
        /// <param name="skillPointsToAdd">Skill points amount to add.</param>
        /// <param name="sendToPlayer">Boolean value that indicates if the system should send the update packet back to the player.</param>
        void AddSkillPoints(ushort skillPointsToAdd, bool sendToPlayer = true);

        /// <summary>
        /// Gets a skill by its id.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <returns>Skill if found; null otherwise.</returns>
        ISkill GetSkill(int skillId);

        /// <summary>
        /// Gets a skill at a given index.
        /// </summary>
        /// <param name="skillIndex">Skill index.</param>
        /// <returns>Skill if found; null otherwise.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the given index is out of range.</exception>
        ISkill GetSkillAtIndex(int skillIndex);

        /// <summary>
        /// Try to get a skill at a given index.
        /// </summary>
        /// <param name="skillIndex">Skill index.</param>
        /// <param name="skill">Outgoing skill instance if found.</param>
        /// <returns>True if the skill is found; false otherwise.</returns>
        bool TryGetSkillAtIndex(int skillIndex, out ISkill skill);

        /// <summary>
        /// Checks if the skill tree contains the skill with the given id.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <returns>True if the skill exists in the skill tree; false otherwise.</returns>
        bool HasSkill(int skillId);

        /// <summary>
        /// Resets the skill tree and redistribute the skill points.
        /// </summary>
        void Reskill();

        /// <summary>
        /// Sets a skill level.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <param name="level">Skill level.</param>
        void SetSkillLevel(int skillId, int level);

        /// <summary>
        /// Initializes and sets the skills in the current skill tree.
        /// </summary>
        /// <param name="skills"></param>
        void SetSkills(IEnumerable<ISkill> skills);

        /// <summary>
        /// Updates the skills levels.
        /// </summary>
        /// <param name="skillsToUpdate">Skills dictionnary to update with skill id as key and skill level as value.</param>
        void Update(IReadOnlyDictionary<int, int> skillsToUpdate);
    }
}
