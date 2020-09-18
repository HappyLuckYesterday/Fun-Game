using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
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
