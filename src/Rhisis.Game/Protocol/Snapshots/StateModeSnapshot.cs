using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class StateModeSnapshot : FFSnapshot
    {
        public StateModeSnapshot(IWorldObject worldObject, StateModeBaseMotion flags, IItem item = null)
            : base(SnapshotType.STATEMODE, worldObject.Id)
        {
            Write((int)worldObject.StateMode);
            Write((byte)flags);

            if (flags == StateModeBaseMotion.BASEMOTION_ON && item != null)
            {
                Write(item.Id);
            }
        }
    }
}
