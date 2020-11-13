using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class MotionSnapshot : FFSnapshot
    {
        public MotionSnapshot(IMover mover, ObjectMessageType objectMessageType)
            : base(SnapshotType.MOTION, mover.Id)
        {
            Write((int)objectMessageType);
        }
    }
}
