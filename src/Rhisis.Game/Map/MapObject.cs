using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Map;

namespace Rhisis.Game.Map
{
    public class MapObject : IMapObject
    {
        public int ModelId { get; set; }

        public Vector3 Position { get; set; }

        public float Angle { get; set; }
    }
}
