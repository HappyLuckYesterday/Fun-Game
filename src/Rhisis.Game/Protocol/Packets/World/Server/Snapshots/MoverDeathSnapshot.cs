using Rhisis.Game.Entities;
using Rhisis.Game.Extensions;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class MoverDeathSnapshot : FFSnapshot
{
    public MoverDeathSnapshot(Mover mover, Mover killer, AttackType attackType)
        : base(SnapshotType.MOVERDEATH, mover.ObjectId)
    {
        var objMsgType = (int)attackType.ToObjectMessageType();
        WriteInt32(objMsgType);
        WriteUInt32(killer.ObjectId);
    }
}
