using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Features.Battle;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots.Battle;

public class MeleeAttackSnapshot : FFSnapshot
{
    public MeleeAttackSnapshot(IMover attacker, IMover target, AttackType attackType, AttackFlags attackFlags)
        : base(SnapshotType.MELEE_ATTACK, attacker.Id)
    {
        int motion = (int)attackType.ToObjectMessageType();
        WriteInt32(motion);
        WriteUInt32(target.Id);
        WriteInt32(0);
        WriteInt32((int)attackFlags);
    }
}
