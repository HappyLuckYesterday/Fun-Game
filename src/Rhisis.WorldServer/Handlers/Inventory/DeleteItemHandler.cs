using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Inventory
{
    [Handler]
    public class DeleteItemHandler
    {
        [HandlerAction(PacketType.REMOVEINVENITEM)]
        public void Execute(IPlayer player, RemoveInventoryItemPacket packet)
        {
            IItem item = player.Inventory.GetItem(packet.ItemIndex);

            if (item == null)
            {
                throw new InvalidOperationException($"Cannot find item with index: '{packet.ItemIndex}'.");
            }

            if (packet.Quantity <= 0)
            {
                throw new ArgumentException("Invalid item quantity to remove.", nameof(packet.Quantity));
            }

            player.Inventory.DeleteItem(item, packet.Quantity);
        }
    }
}
