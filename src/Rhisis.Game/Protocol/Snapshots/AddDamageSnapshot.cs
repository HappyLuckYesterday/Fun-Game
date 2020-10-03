using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class AddDamageSnapshot : FFSnapshot
    {
        public AddDamageSnapshot(IMover mover, IMover attacker, AttackFlags attackFlags, int damage)
            : base(SnapshotType.DAMAGE, mover.Id)
        {
            Write(attacker.Id);
            Write(damage);
            Write((int)attackFlags);

            if (attackFlags.HasFlag(AttackFlags.AF_FLYING))
            {
                Write(mover.DestinationPosition.X);
                Write(mover.DestinationPosition.Y);
                Write(mover.DestinationPosition.Z);
                Write(mover.Angle);
            }
        }
    }
}
