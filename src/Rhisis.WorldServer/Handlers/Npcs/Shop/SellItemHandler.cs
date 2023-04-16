using Rhisis.Game;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Npcs.Shop;

[PacketHandler(PacketType.SELLITEM)]
internal sealed class SellItemHandler : WorldPacketHandler
{
    public void Execute(SellItemPacket packet)
    {
        ItemContainerSlot itemSlot = Player.Inventory.GetAtIndex(packet.ItemIndex);

        if (!itemSlot.HasItem)
        {
            throw new InvalidOperationException($"Cannot find item with index: {packet.ItemIndex} in player's inventory.");
        }

        int sellPrice = itemSlot.Item.Properties.Cost / 4; // TODO: make this configurable
        int sellingQuantity = Math.Min(packet.Quantity, itemSlot.Item.Properties.PackMax);
        int deletedQuantity = Player.Inventory.DeleteItem(itemSlot, sellingQuantity);

        if (deletedQuantity > 0)
        {
            Player.Gold.Increase(sellPrice * deletedQuantity);
        }
    }
}