using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Npcs.Shop;

[PacketHandler(PacketType.OPENSHOPWND)]
internal sealed class OpenShopWindowHandler : WorldPacketHandler
{
    public void Execute(OpenShopWindowPacket packet)
    {
        if (packet.ObjectId <= 0)
        {
            throw new ArgumentException("Invalid object id.");
        }

        Npc npc = Player.VisibleObjects.OfType<Npc>().SingleOrDefault(x => x.ObjectId == packet.ObjectId)
            ?? throw new ArgumentException($"Cannot find NPC with object id: {packet.ObjectId}");

        npc.OpenShop(Player);
        Player.StopMoving();
        Player.CurrentShopName = npc.Name;
    }
}
