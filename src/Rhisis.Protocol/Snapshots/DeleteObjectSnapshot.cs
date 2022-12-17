using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class DeleteObjectSnapshot : FFSnapshot
{
    public DeleteObjectSnapshot(IWorldObject worldObject)
        : base(SnapshotType.DEL_OBJ, worldObject.Id)
    {
    }
}
