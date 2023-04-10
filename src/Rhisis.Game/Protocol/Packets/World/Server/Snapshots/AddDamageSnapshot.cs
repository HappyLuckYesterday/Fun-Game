using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class AddDamageSnapshot : FFSnapshot
{
    public AddDamageSnapshot(Mover mover, Mover attacker, AttackFlags attackFlags, int damage)
        : base(SnapshotType.DAMAGE, mover.ObjectId)
    {
        WriteUInt32(attacker.ObjectId);
        WriteInt32(damage);
        WriteInt32((int)attackFlags);

        if (attackFlags.HasFlag(AttackFlags.AF_FLYING))
        {
            WriteSingle(mover.DestinationPosition.X);
            WriteSingle(mover.DestinationPosition.Y);
            WriteSingle(mover.DestinationPosition.Z);
            WriteSingle(mover.RotationAngle);
        }
    }
}
