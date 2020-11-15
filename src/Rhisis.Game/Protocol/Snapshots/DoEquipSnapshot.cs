using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using System;

namespace Rhisis.Network.Snapshots
{
    public class DoEquipSnapshot : FFSnapshot
    {

        /// <param name="wasEquiped">true: the given item was equiped, false: the item was unequiped</param>
        public DoEquipSnapshot(IPlayer player, IItem item, bool wasEquiped)
            : base(SnapshotType.DOEQUIP, player.Id)
        {
            Write((byte)item.Index);
            Write(0); // Guild id
            Write(Convert.ToByte(wasEquiped));
            Write(item.Id);
            Write(item.Refines);
            Write(0); // See ItemFlags enum
            Write((int)item.Data.Parts);
        }
    }
}
