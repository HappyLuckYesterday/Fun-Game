using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class DeleteObjectSnapshot : FFSnapshot
{
    public DeleteObjectSnapshot(WorldObject worldObject)
        : base(SnapshotType.DEL_OBJ, worldObject.ObjectId)
    {
    }
}
