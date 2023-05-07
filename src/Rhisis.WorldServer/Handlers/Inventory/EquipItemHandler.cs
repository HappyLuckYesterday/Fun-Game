using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.IO;

namespace Rhisis.WorldServer.Handlers.Inventory;

[PacketHandler(PacketType.DOEQUIP)]
internal sealed class EquipItemHandler : WorldPacketHandler
{
    public void Execute(EquipItemPacket packet)
    {
        ItemContainerSlot itemSlot = Player.Inventory.GetAtIndex(packet.ItemIndex);
        ItemPartType parts = packet.Part;

        if (!itemSlot.HasItem)
        {
            throw new InvalidOperationException($"Item slot at index '{packet.ItemIndex}' doesn't have any item.");
        }

        if (itemSlot.Number > Rhisis.Game.Inventory.EquipOffset)
        {
            if (parts != itemSlot.Item.Properties.Parts)
            {
                throw new InvalidDataException($"Item parts doesn't match client parts.");
            }

            Player.Inventory.Unequip(itemSlot);
        }
        else
        {
            Player.Inventory.Equip(itemSlot);
        }

        Player.Defense.Update();
    }
}
