using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class CreateItemSnapshot : FFSnapshot
{
    public CreateItemSnapshot(Player player, Item item, int indexContainerIndex)
        : base(SnapshotType.CREATEITEM, player.ObjectId)
    {
        WriteByte(0);
        WriteInt32(-1);
        item.Serialize(this);
        WriteByte(1);
        WriteByte((byte)indexContainerIndex);
        WriteInt16((short)item.Quantity);
    }
}
