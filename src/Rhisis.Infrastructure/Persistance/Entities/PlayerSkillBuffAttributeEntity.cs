using Rhisis.Game.Common;

namespace Rhisis.Infrastructure.Persistance.Entities;

public class PlayerSkillBuffAttributeEntity
{
    /// <summary>
    /// Gets or sets the player skill buff id.
    /// </summary>
    public int PlayerSkillBuffId { get; set; }

    /// <summary>
    /// Gets or sets the player skill buff instance.
    /// </summary>
    public PlayerSkillBuffEntity PlayerBuff { get; set; }

    /// <summary>
    /// Gets or sets the buff attribute.
    /// </summary>
    public DefineAttributes Attribute { get; set; }

    /// <summary>
    /// Gets or sets the buff attribute value.
    /// </summary>
    public int Value { get; set; }
}