using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
{
    public class DeleteObjectSnapshot : FFSnapshot
    {
        public DeleteObjectSnapshot(IWorldObject worldObject)
            : base(SnapshotType.DEL_OBJ, worldObject.Id)
        {
        }
    }
}
