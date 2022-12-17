﻿namespace Rhisis.Infrastructure.Persistance.Entities;

public class DbSkill
{
    /// <summary>
    /// Gets or sets the skill row id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the skill id.
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets the skill level.
    /// </summary>
    public byte Level { get; set; }

    /// <summary>
    /// Gets or sets the character id.
    /// </summary>
    public int CharacterId { get; set; }

    /// <summary>
    /// Gets or sets the character.
    /// </summary>
    public DbCharacter Character { get; set; }
}
