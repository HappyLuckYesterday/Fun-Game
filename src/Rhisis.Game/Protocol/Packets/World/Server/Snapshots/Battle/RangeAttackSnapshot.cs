using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Extensions;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Battle;

public class RangeAttackSnapshot : FFSnapshot
{
    public RangeAttackSnapshot(Mover attacker, AttackType rangeAttackType, uint targetId, int power, int projectileId)
        : base(SnapshotType.RANGE_ATTACK, attacker.ObjectId)
    {
        int motion = (int)rangeAttackType.ToObjectMessageType();

        WriteInt32(motion);
        WriteUInt32(targetId);
        WriteInt32(power);
        WriteInt32(0); // unused parameter, always 0
        WriteInt32(projectileId);
    }
}
