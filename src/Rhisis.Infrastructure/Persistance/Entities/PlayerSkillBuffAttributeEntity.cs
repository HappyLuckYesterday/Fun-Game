using Rhisis.Game.Common;

namespace Rhisis.Infrastructure.Persistance.Entities;

public class PlayerSkillBuffAttributeEntity
{
    /// <summary>
    /// Gets or sets the player id.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the skill id.
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets the buff attribute.
    /// </summary>
    public DefineAttributes Attribute { get; set; }

    /// <summary>
    /// Gets or sets the buff attribute value.
    /// </summary>
    public int Value { get; set; }
}