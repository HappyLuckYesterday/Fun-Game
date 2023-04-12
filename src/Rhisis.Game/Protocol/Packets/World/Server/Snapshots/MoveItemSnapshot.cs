using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class MoveItemSnapshot : FFSnapshot
{
    public MoveItemSnapshot(Player player, int sourceSlot, int destinationSlot)
        : base(SnapshotType.MOVEITEM, player.ObjectId)
    {
        WriteByte(0); // item type (not used)
        WriteByte((byte)sourceSlot);
        WriteByte((byte)destinationSlot);
    }
}
