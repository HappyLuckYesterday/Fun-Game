using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class MoverSetDestObjectSnapshot : FFSnapshot
{
    public MoverSetDestObjectSnapshot(Mover mover, WorldObject target, float distance = 1f)
        : base(SnapshotType.MOVERSETDESTOBJ, mover.ObjectId)
    {
        WriteUInt32(target.ObjectId);
        WriteSingle(distance);
    }
}
