using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Rhisis.Game.IO.Rgn;

namespace Rhisis.Game.Map
{
    public class MapRespawnRegion : MapRegion, IMapRespawnRegion
    {
        public WorldObjectType ObjectType { get; private set; }

        public int ModelId { get; private set; }

        public int Time { get; private set; }

        public int Count { get; private set; }

        public float Height { get; private set; }

        public MapRespawnRegion(int x, int z, int width, int length, int time, float height, WorldObjectType type, int modelId, int count)
            : base(x, z, width, length)
        {
            ModelId = modelId;
            Time = time;
            ObjectType = type;
            Count = count;
            Height = height;
        }

        public override object Clone()
        {
            var region = new MapRespawnRegion(X, Z, Width, Length, Time, Height, ObjectType, ModelId, Count)
            {
                IsActive = IsActive
            };

            return region;
        }

        public static IMapRespawnRegion FromRgnElement(RgnRespawn7 region)
        {
            return new MapRespawnRegion(region.Left, region.Top, region.Right - region.Left, region.Bottom - region.Top,
                region.Time, region.Position.Y, (WorldObjectType)region.Type, region.Model, region.Count);
        }
    }
}
