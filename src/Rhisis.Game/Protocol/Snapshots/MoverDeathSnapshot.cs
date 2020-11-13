using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class MoverDeathSnapshot : FFSnapshot
    {
        public MoverDeathSnapshot(IMover mover, IMover killer, ObjectMessageType objectMessageType)
            : base(SnapshotType.MOVERDEATH, mover.Id)
        {
            Write((int)objectMessageType);
            Write(killer.Id);
        }
    }
}
