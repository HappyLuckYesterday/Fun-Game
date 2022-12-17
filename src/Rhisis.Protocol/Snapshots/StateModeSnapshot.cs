using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class StateModeSnapshot : FFSnapshot
{
    public StateModeSnapshot(IWorldObject worldObject, StateModeBaseMotion flags, IItem item = null)
        : base(SnapshotType.STATEMODE, worldObject.Id)
    {
        WriteInt32((int)worldObject.StateMode);
        WriteByte((byte)flags);

        if (flags == StateModeBaseMotion.BASEMOTION_ON && item != null)
        {
            WriteInt32(item.Id);
        }
    }
}
