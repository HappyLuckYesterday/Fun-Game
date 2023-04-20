using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SetSpeedFactorSnapshot : FFSnapshot
{
    public SetSpeedFactorSnapshot(Mover mover, float speedFactor)
        : base(SnapshotType.SET_SPEED_FACTOR, mover.ObjectId)
    {
        WriteSingle(speedFactor);
    }
}
