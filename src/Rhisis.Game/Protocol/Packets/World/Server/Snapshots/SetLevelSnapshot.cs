using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SetLevelSnapshot : FFSnapshot
{
    public SetLevelSnapshot(Player player, int level)
        : base(SnapshotType.SETLEVEL, player.ObjectId)
    {
        WriteInt16((short)level);
    }
}
