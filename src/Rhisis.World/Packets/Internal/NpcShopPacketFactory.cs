using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class NpcShopPacketFactory : PacketFactoryBase, INpcShopPacketFactory
    {
        /// <inheritdoc />
        public void SendOpenNpcShop(IPlayerEntity player, INpcEntity npc)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(npc.Id, SnapshotType.OPENSHOPWND);

            foreach (NpcShopItemContainerComponent shopTab in npc.Shop)
            {
                shopTab.Serialize(packet);
            }

            SendToPlayer(player, packet);
        }
    }
}
