using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class WorldReadInfoSnapshot : FFSnapshot
{
    public WorldReadInfoSnapshot(WorldObject worldObject)
        : base(SnapshotType.WORLD_READINFO, worldObject.ObjectId)
    {
        WriteInt32(worldObject.Map.Id);
        WriteSingle(worldObject.Position.X);
        WriteSingle(worldObject.Position.Y);
        WriteSingle(worldObject.Position.Z);
    }
}
