using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
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
