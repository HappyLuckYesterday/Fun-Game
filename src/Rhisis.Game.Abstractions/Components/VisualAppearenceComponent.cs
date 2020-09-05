using Rhisis.Core.Common;

namespace Rhisis.Game.Abstractions.Components
{
    public class VisualAppearenceComponent
    {
        public GenderType Gender { get; }

        public int SkinSetId { get; set; }

        public int HairId { get; set; }

        public int HairColor { get; set; }

        public int FaceId { get; set; }

        public VisualAppearenceComponent(GenderType gender)
        {
            Gender = gender;
        }
    }
}
