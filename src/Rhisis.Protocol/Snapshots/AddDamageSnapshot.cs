using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class AddDamageSnapshot : FFSnapshot
{
    public AddDamageSnapshot(IMover mover, IMover attacker, AttackFlags attackFlags, int damage)
        : base(SnapshotType.DAMAGE, mover.Id)
    {
        WriteUInt32(attacker.Id);
        WriteInt32(damage);
        WriteInt32((int)attackFlags);

        if (attackFlags.HasFlag(AttackFlags.AF_FLYING))
        {
            WriteSingle(mover.DestinationPosition.X);
            WriteSingle(mover.DestinationPosition.Y);
            WriteSingle(mover.DestinationPosition.Z);
            WriteSingle(mover.Angle);
        }
    }
}
