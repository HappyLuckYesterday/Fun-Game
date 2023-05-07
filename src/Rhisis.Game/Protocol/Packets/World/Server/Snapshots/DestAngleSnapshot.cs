using Rhisis.Game.Entities;

namespace Rhisis.Protocol.Snapshots;

public class DestAngleSnapshot : FFSnapshot
{
    public DestAngleSnapshot(Mover mover, bool left = false)
        : base(SnapshotType.DESTANGLE, mover.ObjectId)
    {
        WriteSingle(mover.RotationAngle);
        WriteBoolean(left);
    }
}