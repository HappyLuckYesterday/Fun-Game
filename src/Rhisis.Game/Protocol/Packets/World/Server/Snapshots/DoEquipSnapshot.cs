using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class DoEquipSnapshot : FFSnapshot
{
    public DoEquipSnapshot(Player player, Item item, int itemIndex, bool wasEquiped)
        : base(SnapshotType.DOEQUIP, player.ObjectId)
    {
        WriteByte((byte)itemIndex);
        WriteInt32(0); // Guild id
        WriteByte(Convert.ToByte(wasEquiped));
        WriteInt32(item.Id);
        WriteInt32(item.Refines);
        WriteInt32(0); // See ItemFlags enum
        WriteInt32((int)item.Properties.Parts);
    }
}
