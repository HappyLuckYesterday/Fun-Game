﻿using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class TradePacketFactory : PacketFactoryBase, ITradePacketFactory
    {
        /// <inheritdoc />
        public void SendTradeRequest(IPlayerEntity player, IPlayerEntity target)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.CONFIRMTRADE);

            SendToPlayer(target, packet);
        }

        /// <inheritdoc />
        public void SendTradeRequestCancel(IPlayerEntity player, IPlayerEntity target)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.CONFIRMTRADECANCEL);

            SendToPlayer(target, packet);
        }

        /// <inheritdoc />
        public void SendTrade(IPlayerEntity player, IPlayerEntity target, uint playerId)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(target.Id, SnapshotType.TRADE);
            packet.Write(playerId);
            target.Inventory.Serialize(packet);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradePut(IPlayerEntity player, IPlayerEntity trader, byte slot, byte itemType, byte id, short count)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(trader.Id, SnapshotType.TRADEPUT);
            packet.Write(slot);
            packet.Write(itemType);
            packet.Write(id);
            packet.Write(count);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradePutError(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.TRADEPUTERROR);

                SendToPlayer(player, packet);
            }
        }

        /// <inheritdoc />
        public void SendTradePutGold(IPlayerEntity player, IPlayerEntity trader, int gold)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(trader.Id, SnapshotType.TRADEPUTGOLD);
            packet.Write(gold);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradeCancel(IPlayerEntity player, int mode)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.TRADECANCEL);
            packet.Write(player.Id);
            packet.Write(mode);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradeOk(IPlayerEntity player, uint traderId)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(traderId, SnapshotType.TRADEOK);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradeLastConfirm(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(0, SnapshotType.TRADELASTCONFIRM);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradeLastConfirmOk(IPlayerEntity player, uint traderId)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(traderId, SnapshotType.TRADELASTCONFIRMOK);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendTradeConsent(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(0, SnapshotType.TRADECONSENT);

            SendToPlayer(player, packet);
        }
    }
}
