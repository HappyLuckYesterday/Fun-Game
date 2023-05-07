using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Extensions;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Battle;

public class MeleeAttackSnapshot : FFSnapshot
{
    public MeleeAttackSnapshot(Mover attacker, Mover target, AttackType attackType, AttackFlags attackFlags)
        : base(SnapshotType.MELEE_ATTACK, attacker.ObjectId)
    {
        int motion = (int)attackType.ToObjectMessageType();
        WriteInt32(motion);
        WriteUInt32(target.ObjectId);
        WriteInt32(0);
        WriteInt32((int)attackFlags);
    }
}
