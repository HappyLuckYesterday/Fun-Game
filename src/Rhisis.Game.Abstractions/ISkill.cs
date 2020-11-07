using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common.Resources;
using System;

namespace Rhisis.Game.Abstractions
{
    public interface ISkill : IPacketSerializer, IEquatable<ISkill>
    {
        /// <summary>
        /// Gets the skill id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the skill owner.
        /// </summary>
        IMover Owner { get; }

        /// <summary>
        /// Gets the skill database id.
        /// </summary>
        int? DatabaseId { get; }

        /// <summary>
        /// Gets or sets the skill level.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Gets the skill name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the skill data.
        /// </summary>
        SkillData Data { get; }

        /// <summary>
        /// Gets the skill level data for the current level.
        /// </summary>
        SkillLevelData LevelData { get; }

        /// <summary>
        /// Sets the skill cool time before it can be used again.
        /// </summary>
        /// <param name="coolTime">Cool time.</param>
        void SetCoolTime(long coolTime);

        /// <summary>
        /// Check if the current skill cool time is over.
        /// </summary>
        /// <returns>True if the cooltime is over; false otherwise.</returns>
        bool IsCoolTimeElapsed();

        bool CanUse(IMover target = null);

        void Use(IMover target = null);

        /// <summary>
        /// Gets the skill casting time.
        /// </summary>
        /// <returns>Skill casting time in milliseconds.</returns>
        int GetCastingTime();
    }
}
