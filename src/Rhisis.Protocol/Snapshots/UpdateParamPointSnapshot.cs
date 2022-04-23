using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots
{
    public class UpdateParamPointSnapshot : FFSnapshot
    {
        public UpdateParamPointSnapshot(IWorldObject worldObject, DefineAttributes attribute, int value)
            : base(SnapshotType.SETPOINTPARAM, worldObject.Id)
        {
            WriteInt32((int)attribute);
            WriteInt32(value);
        }
    }
}
