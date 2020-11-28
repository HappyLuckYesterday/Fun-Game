using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.NpcShop
{
    [Handler]
    public class OpenNpcShopHandler
    {
        [HandlerAction(PacketType.OPENSHOPWND)]
        public void Execute(IPlayer player, OpenShopWindowPacket packet)
        {
            if (packet.ObjectId <= 0)
            {
                throw new ArgumentException("Invalid object id.");
            }

            INpc npc = player.VisibleObjects.OfType<INpc>().SingleOrDefault(x => x.Id == packet.ObjectId);

            if (npc == null)
            {
                throw new ArgumentException($"Cannot find NPC with object id: {packet.ObjectId}");
            }

            if (npc.HasShop)
            {
                player.CurrentNpcShopName = npc.Name;

                using var openNpcShopSnapshot = new OpenNpcShopWindowSnapshot(npc, npc.Shop);

                player.Connection.Send(openNpcShopSnapshot);
            }
        }
    }
}
