using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class UpdateItemSnapshot : FFSnapshot
{
    public UpdateItemSnapshot(Player player, UpdateItemType updateItemType, int itemIndex, int value)
        : base(SnapshotType.UPDATE_ITEM, player.ObjectId)
    {
        WriteByte(0);
        WriteByte((byte)itemIndex);
        WriteByte((byte)updateItemType);
        WriteInt32(value);
        WriteInt32(0); // time
    }
}
