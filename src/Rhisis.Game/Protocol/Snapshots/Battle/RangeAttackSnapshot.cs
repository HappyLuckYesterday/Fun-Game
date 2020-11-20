using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Battle
{
    public class RangeAttackSnapshot : FFSnapshot
    {
        public RangeAttackSnapshot(IMover attacker, AttackType rangeAttackType, uint targetId, int power, int projectileId)
            : base(SnapshotType.RANGE_ATTACK, attacker.Id)
        {
            int motion = (int)rangeAttackType.ToObjectMessageType();

            WriteInt32(motion);
            WriteUInt32(targetId);
            WriteInt32(power);
            WriteInt32(0); // unused parameter, always 0
            WriteInt32(projectileId);
        }
    }
}
