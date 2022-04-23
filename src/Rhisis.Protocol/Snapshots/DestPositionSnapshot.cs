using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class DestPositionSnapshot : FFSnapshot
    {
        public DestPositionSnapshot(IMover mover)
            : base(SnapshotType.DESTPOS, mover.Id)
        {
            WriteSingle(mover.DestinationPosition.X);
            WriteSingle(mover.DestinationPosition.Y);
            WriteSingle(mover.DestinationPosition.Z);
            WriteByte(1);
        }
    }
}
