using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.IO;

namespace Rhisis.WorldServer.Handlers.Inventory
{
    [Handler]
    public class EquipItemHandler
    {
        [HandlerAction(PacketType.DOEQUIP)]
        public void Execute(IPlayer player, EquipItemPacket packet)
        {
            IItem item = player.Inventory.GetItem(packet.ItemIndex);

            if (item == null)
            {
                throw new InvalidOperationException($"Cannot find item with index: '{packet.ItemIndex}'.");
            }

            var parts = (ItemPartType)packet.Part;

            if (player.Inventory.IsItemEquiped(item))
            {
                if (parts != item.Data.Parts)
                {
                    throw new InvalidDataException($"Item parts doesn't match client parts.");
                }

                player.Inventory.Unequip(item);
            }
            else
            {
                player.Inventory.Equip(item);
            }

            player.Defense.Update();
        }
    }
}
