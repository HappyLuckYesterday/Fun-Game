using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class UpdateDestParamSnapshot : FFSnapshot
{
    public UpdateDestParamSnapshot(WorldObject worldObject, DefineAttributes attribute, int value)
        : base(SnapshotType.SETDESTPARAM, worldObject.ObjectId)
    {
        WriteInt32((int)attribute);
        WriteInt32(value);
    }
}
