using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;

namespace Rhisis.Game.Components
{
    public class HumanVisualAppearenceComponent : IHumanVisualAppearance
    {
        public GenderType Gender { get; }

        public int SkinSetId { get; set; }

        public int HairId { get; set; }

        public int HairColor { get; set; }

        public int FaceId { get; set; }

        public HumanVisualAppearenceComponent(GenderType gender)
        {
            Gender = gender;
        }
    }
}
