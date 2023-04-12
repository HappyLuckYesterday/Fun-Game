using Rhisis.Game.Common;

namespace Rhisis.Game;

public sealed class HumanVisualAppearance
{
    /// <summary>
    /// Gets the human's gender.
    /// </summary>
    public GenderType Gender { get; init; }

    /// <summary>
    /// Gets the human's skin set id.
    /// </summary>
    public int SkinSetId { get; init; }

    /// <summary>
    /// Gets or sets the human's hair model id.
    /// </summary>
    public int HairId { get; set; }

    /// <summary>
    /// Gets or sets the human's hair color.
    /// </summary>
    public int HairColor { get; set; }

    /// <summary>
    /// Gets or sets the human's face id.
    /// </summary>
    public int FaceId { get; set; }
}