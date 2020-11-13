using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
{
    public class WorldReadInfoSnapshot : FFSnapshot
    {
        public WorldReadInfoSnapshot(IWorldObject worldObject)
            : base(SnapshotType.WORLD_READINFO, worldObject.Id)
        {
            Write(worldObject.Map.Id);
            Write(worldObject.Position.X);
            Write(worldObject.Position.Y);
            Write(worldObject.Position.Z);
        }
    }
}
