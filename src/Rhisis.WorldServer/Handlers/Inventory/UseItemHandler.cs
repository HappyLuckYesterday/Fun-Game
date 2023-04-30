using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Inventory;

[PacketHandler(PacketType.DOUSEITEM)]
internal sealed class UseItemHandler : WorldPacketHandler
{
    public void Execute(DoUseItemPacket packet)
    {
        ItemContainerSlot itemSlot = Player.Inventory.GetAtIndex(packet.ItemIndex);

        if (itemSlot is not null && itemSlot.HasItem)
        {
            if (packet.Part > 0)
            {
                if (packet.Part >= (int)ItemPartType.Maximum)
                {
                    throw new InvalidOperationException($"Invalid equipement part.");
                }

                Player.Inventory.Equip(itemSlot.Item);
            }
            else
            {
                Player.Inventory.UseItem(itemSlot.Item);
            }
        }
    }
}