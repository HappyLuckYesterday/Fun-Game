using System.Collections.Generic;

namespace Rhisis.Infrastructure.Persistance.Entities;

public class PlayerSkillBuffEntity
{
    /// <summary>
    /// Gets or sets the skill buff id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the skill buff remaining time.
    /// </summary>
    public int RemainingTime { get; set; }

    /// <summary>
    /// Gets or sets the buff skill id.
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets the buff skill level.
    /// </summary>
    public int SkillLevel { get; set; }

    /// <summary>
    /// Gets or sets the player id.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the player instance.
    /// </summary>
    public PlayerEntity Player { get; set; }

    /// <summary>
    /// Gets or sets the buff skill attributes.
    /// </summary>
    public ICollection<PlayerSkillBuffAttributeEntity> Attributes { get; set; }
}
