using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;
using System;

namespace Rhisis.Protocol.Snapshots;

public class DoEquipSnapshot : FFSnapshot
{

    /// <param name="wasEquiped">true: the given item was equiped, false: the item was unequiped</param>
    public DoEquipSnapshot(IPlayer player, IItem item, bool wasEquiped)
        : base(SnapshotType.DOEQUIP, player.Id)
    {
        WriteByte((byte)item.Index);
        WriteInt32(0); // Guild id
        WriteByte(Convert.ToByte(wasEquiped));
        WriteInt32(item.Id);
        WriteInt32(item.Refines);
        WriteInt32(0); // See ItemFlags enum
        WriteInt32((int)item.Data.Parts);
    }
}
