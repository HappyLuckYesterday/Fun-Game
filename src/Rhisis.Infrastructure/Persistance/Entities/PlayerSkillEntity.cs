namespace Rhisis.Infrastructure.Persistance.Entities;

public class PlayerSkillEntity
{
    /// <summary>
    /// Gets or sets the skill id.
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets the skill level.
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
}