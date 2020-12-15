using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class UpdateDestParamSnapshot : FFSnapshot
    {
        public UpdateDestParamSnapshot(IWorldObject worldObject, DefineAttributes attribute, int value)
            : base(SnapshotType.SETDESTPARAM, worldObject.Id)
        {
            Write((int)attribute);
            Write(value);
        }
    }
}
