using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class UpdateParamPointSnapshot : FFSnapshot
    {
        public UpdateParamPointSnapshot(IWorldObject worldObject, DefineAttributes attribute, int value)
            : base(SnapshotType.SETPOINTPARAM, worldObject.Id)
        {
            Write((int)attribute);
            Write(value);
        }
    }
}
