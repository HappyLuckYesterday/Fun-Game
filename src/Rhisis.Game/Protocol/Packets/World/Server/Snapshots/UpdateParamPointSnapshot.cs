using Rhisis.Game;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class UpdateParamPointSnapshot : FFSnapshot
{
    public UpdateParamPointSnapshot(WorldObject worldObject, DefineAttributes attribute, int value)
        : base(SnapshotType.SETPOINTPARAM, worldObject.ObjectId)
    {
        WriteInt32((int)attribute);
        WriteInt32(value);
    }
}
