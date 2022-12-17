using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Features.Battle;

namespace Rhisis.Protocol.Snapshots;

public class MoverDeathSnapshot : FFSnapshot
{
    public MoverDeathSnapshot(IMover mover, IMover killer, AttackType attackType)
        : base(SnapshotType.MOVERDEATH, mover.Id)
    {
        var objMsgType = (int)attackType.ToObjectMessageType();
        WriteInt32(objMsgType);
        WriteUInt32(killer.Id);
    }
}
