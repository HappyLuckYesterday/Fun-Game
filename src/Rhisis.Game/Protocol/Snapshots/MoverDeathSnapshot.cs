using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class MoverDeathSnapshot : FFSnapshot
    {
        public MoverDeathSnapshot(IMover mover, IMover killer, AttackType attackType)
            : base(SnapshotType.MOVERDEATH, mover.Id)
        {
            var objMsgType = (int)attackType.ToObjectMessageType();
            Write(objMsgType);
            Write(killer.Id);
        }
    }
}
