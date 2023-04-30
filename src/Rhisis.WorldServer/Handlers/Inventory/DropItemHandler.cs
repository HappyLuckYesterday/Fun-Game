using Rhisis.Game;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Inventory;

[PacketHandler(PacketType.DROPITEM)]
internal sealed class DropItemHandler : WorldPacketHandler
{
    public void Execute(DropItemPacket packet)
    {
        if (packet.ItemQuantity <= 0)
        {
            throw new InvalidOperationException("Invalid drop quantity.");
        }

        ItemContainerSlot itemSlot = Player.Inventory.GetAtIndex(packet.ItemIndex) ??
            throw new InvalidOperationException($"Cannot find item slot with index: {packet.ItemIndex} in player's inventory.");

        if (!itemSlot.HasItem)
        {
            throw new InvalidOperationException("Cannot drop an item of an empty slot.");
        }

        int quantityToDrop = Math.Min(packet.ItemQuantity, itemSlot.Item.Quantity);
        Item droppedItem = itemSlot.Item.Clone();
        droppedItem.Quantity = quantityToDrop;

        if (Player.Inventory.DeleteItem(itemSlot, quantityToDrop) > 0)
        {
            Player.DropItem(droppedItem);
        }
    }
}