using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class WorldReadInfoSnapshot : FFSnapshot
    {
        public WorldReadInfoSnapshot(IWorldObject worldObject)
            : base(SnapshotType.WORLD_READINFO, worldObject.Id)
        {
            WriteInt32(worldObject.Map.Id);
            WriteSingle(worldObject.Position.X);
            WriteSingle(worldObject.Position.Y);
            WriteSingle(worldObject.Position.Z);
        }
    }
}
