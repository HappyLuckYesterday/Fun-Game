using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class DestPositionSnapshot : FFSnapshot
{
    public DestPositionSnapshot(Mover mover)
        : base(SnapshotType.DESTPOS, mover.ObjectId)
    {
        WriteSingle(mover.DestinationPosition.X);
        WriteSingle(mover.DestinationPosition.Y);
        WriteSingle(mover.DestinationPosition.Z);
        WriteByte(1);
    }
}
