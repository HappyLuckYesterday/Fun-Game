using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class UpdateDestParamSnapshot : FFSnapshot
{
    public UpdateDestParamSnapshot(IWorldObject worldObject, DefineAttributes attribute, int value)
        : base(SnapshotType.SETDESTPARAM, worldObject.Id)
    {
        WriteInt32((int)attribute);
        WriteInt32(value);
    }
}
