using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Inventory
{
    [Handler]
    public class MoveItemHandler
    {
        [HandlerAction(PacketType.MOVEITEM)]
        public void Execute(IPlayer player, MoveItemPacket packet)
        {
            if (packet.SourceSlot < 0 || packet.SourceSlot >= player.Inventory.MaxCapacity)
            {
                throw new InvalidOperationException("Source slot is out of inventory range.");
            }

            if (packet.DestinationSlot < 0 || packet.DestinationSlot >= player.Inventory.MaxCapacity)
            {
                throw new InvalidOperationException("Destination slot is out of inventory range.");
            }

            if (packet.SourceSlot == packet.DestinationSlot)
            {
                return;
            }

            player.Inventory.MoveItem(packet.SourceSlot, packet.DestinationSlot);
        }
    }
}
