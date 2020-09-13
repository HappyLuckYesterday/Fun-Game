using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Components
{
    public interface IHumanVisualAppearance
    {
        /// <summary>
        /// Gets the human's gender.
        /// </summary>
        GenderType Gender { get; }

        /// <summary>
        /// Gets or sets the human's skin set id.
        /// </summary>
        int SkinSetId { get; set; }

        /// <summary>
        /// Gets or sets the human's hair model id.
        /// </summary>
        int HairId { get; set; }

        /// <summary>
        /// Gets or sets the human's hair color.
        /// </summary>
        int HairColor { get; set; }

        /// <summary>
        /// Gets or sets the human's face id.
        /// </summary>
        int FaceId { get; set; }
    }
}
