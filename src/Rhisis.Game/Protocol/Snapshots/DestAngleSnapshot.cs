using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
{
    public class DestAngleSnapshot : FFSnapshot
    {
        public DestAngleSnapshot(IMover mover, bool left = false)
            : base(SnapshotType.DESTANGLE, mover.Id)
        {
            Write(mover.Angle);
            Write(left);
        }
    }
}
