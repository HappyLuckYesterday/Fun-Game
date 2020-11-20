using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Battle
{
    public class MeleeAttackSnapshot : FFSnapshot
    {
        public MeleeAttackSnapshot(IMover attacker, IMover target, AttackType attackType, AttackFlags attackFlags)
            : base(SnapshotType.MELEE_ATTACK, attacker.Id)
        {
            int motion = (int)attackType.ToObjectMessageType();
            Write(motion);
            Write(target.Id);
            Write(0);
            Write((int)attackFlags);
        }
    }
}
