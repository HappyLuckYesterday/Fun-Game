using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class MoverSetDestObjectSnapshot : FFSnapshot
    {
        public MoverSetDestObjectSnapshot(IMover mover, IWorldObject target, float distance = 1f)
            : base(SnapshotType.MOVERSETDESTOBJ, mover.Id)
        {
            Write(target.Id);
            Write(distance);
        }
    }
}
