using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class MotionSnapshot : FFSnapshot
{
    public MotionSnapshot(IMover mover, ObjectMessageType objectMessageType)
        : base(SnapshotType.MOTION, mover.Id)
    {
        WriteInt32((int)objectMessageType);
    }
}
