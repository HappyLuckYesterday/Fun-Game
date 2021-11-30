using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class DestPositionSnapshot : FFSnapshot
    {
        public DestPositionSnapshot(IMover mover)
            : base(SnapshotType.DESTPOS, mover.Id)
        {
            Write(mover.DestinationPosition.X);
            Write(mover.DestinationPosition.Y);
            Write(mover.DestinationPosition.Z);
            Write<byte>(1);
        }
    }
}
