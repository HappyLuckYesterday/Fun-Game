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
        ItemContainerSlot slot = Player.Inventory.GetAtIndex(packet.ItemIndex);

        if (slot is not null && slot.HasItem)
        {
            if (packet.Part > 0)
            {
                if (packet.Part >= (int)ItemPartType.Maximum)
                {
                    throw new InvalidOperationException($"Invalid equipement part.");
                }

                Player.Inventory.Equip(slot.Item);
            }
            else
            {
                Player.Inventory.UseItem(slot.Item);
            }
        }
    }
}