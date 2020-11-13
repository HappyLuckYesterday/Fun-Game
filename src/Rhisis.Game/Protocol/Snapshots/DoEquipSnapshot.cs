using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using System;

namespace Rhisis.Network.Snapshots
{
    public class DoEquipSnapshot : FFSnapshot
    {
        public DoEquipSnapshot(IPlayer player, IItem item, bool equip)
            : base(SnapshotType.DOEQUIP, player.Id)
        {
            Write((byte)item.Index);
            Write(0); // Guild id
            Write(Convert.ToByte(equip));
            Write(item.Id);
            Write(item.Refines);
            Write(0); // flag
            Write((int)item.Data.Parts);
        }
    }
}
