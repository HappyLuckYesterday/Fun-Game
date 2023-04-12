using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers.Inventory;

[PacketHandler(PacketType.MOVEITEM)]
internal sealed class MoveItemHandler : WorldPacketHandler
{
    public void Execute(MoveItemPacket packet)
    {
        Player.Inventory.MoveItem(packet.SourceSlot, packet.DestinationSlot);
    }
}
