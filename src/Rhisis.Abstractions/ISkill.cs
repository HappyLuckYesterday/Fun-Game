﻿using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using System;

namespace Rhisis.Abstractions;

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
    /// Gets the skill casting time.
    /// </summary>
    /// <returns>Skill casting time in milliseconds.</returns>
    int GetCastingTime();

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

    /// <summary>
    /// Checks if the player can use the skill.
    /// </summary>
    /// <param name="target">Skill target.</param>
    /// <returns>True if the player can use the skill; false otherwise.</returns>
    bool CanUse(IMover target);

    /// <summary>
    /// Use a skill.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="skillUseType">Skill usage type.</param>
    void Use(IMover target, SkillUseType skillUseType = SkillUseType.Normal);
}
