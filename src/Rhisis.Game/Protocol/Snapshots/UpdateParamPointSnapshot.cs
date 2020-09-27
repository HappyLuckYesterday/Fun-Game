using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
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
