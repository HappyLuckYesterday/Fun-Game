using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class DestAngleSnapshot : FFSnapshot
{
    public DestAngleSnapshot(IMover mover, bool left = false)
        : base(SnapshotType.DESTANGLE, mover.Id)
    {
        WriteSingle(mover.Angle);
        WriteBoolean(left);
    }
}
