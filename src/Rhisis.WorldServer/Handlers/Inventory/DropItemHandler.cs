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

        ItemContainerSlot slot = Player.Inventory.GetAtIndex(packet.ItemIndex);

        if (!slot.HasItem)
        {
            throw new InvalidOperationException($"Cannot find item with index: {packet.ItemIndex} in player's inventory.");
        }

        int quantityToDrop = Math.Min(packet.ItemQuantity, slot.Item.Quantity);
        Item droppedItem = slot.Item.Clone();
        droppedItem.Quantity = quantityToDrop;

        if (Player.Inventory.DeleteItem(slot, quantityToDrop) > 0)
        {
            Player.DropItem(droppedItem);
        }
    }
}