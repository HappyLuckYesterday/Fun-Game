using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class DeleteObjectSnapshot : FFSnapshot
    {
        public DeleteObjectSnapshot(IWorldObject worldObject)
            : base(SnapshotType.DEL_OBJ, worldObject.Id)
        {
        }
    }
}
