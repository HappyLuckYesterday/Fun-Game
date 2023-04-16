using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers.Npcs.Shop;

[PacketHandler(PacketType.CLOSESHOPWND)]
internal sealed class CloseShopWindowHandler : WorldPacketHandler
{
    public void Execute(CloseShopWindowPacket packet)
    {
        Player.CurrentShopName = null;
    }
}