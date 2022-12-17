using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class MoverSetDestObjectSnapshot : FFSnapshot
{
    public MoverSetDestObjectSnapshot(IMover mover, IWorldObject target, float distance = 1f)
        : base(SnapshotType.MOVERSETDESTOBJ, mover.Id)
    {
        WriteUInt32(target.Id);
        WriteSingle(distance);
    }
}
