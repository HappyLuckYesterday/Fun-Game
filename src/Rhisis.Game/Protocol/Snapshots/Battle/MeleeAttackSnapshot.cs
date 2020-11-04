using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Battle
{
    public class MeleeAttackSnapshot : FFSnapshot
    {
        public MeleeAttackSnapshot(IMover attacker, IMover target, ObjectMessageType motion, AttackFlags attackFlags)
            : base(SnapshotType.MELEE_ATTACK, attacker.Id)
        {
            Write((int)motion);
            Write(target.Id);
            Write(0);
            Write((int)attackFlags);
        }
    }
}
