using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class SetSpeedFactorSnapshot : FFSnapshot
    {
        public SetSpeedFactorSnapshot(IMover mover, float speedFactor)
            : base(SnapshotType.SET_SPEED_FACTOR, mover.Id)
        {
            Write(speedFactor);
        }
    }
}
