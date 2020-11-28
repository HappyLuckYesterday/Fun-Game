using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.NpcShop
{
    [Handler]
    public class SellItemHandler
    {
        [HandlerAction(PacketType.SELLITEM)]
        public void Execute(IPlayer player, SellItemPacket packet)
        {
            IItem item = player.Inventory.GetItem(packet.ItemIndex);

            if (item == null)
            {
                throw new InvalidOperationException($"Cannot find item with index: {packet.ItemIndex} in player's inventory.");
            }

            int sellingQuantity = Math.Min(packet.Quantity, item.Data.PackMax);
            int sellPrice = item.Data.Cost / 4; // TODO: make this configurable
            int deletedQuantity = player.Inventory.DeleteItem(item, sellingQuantity);

            if (deletedQuantity > 0)
            {
                player.Gold.Increase(sellPrice * deletedQuantity);
            }
        }
    }
}
