﻿using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class PlayerDataPacketFactory : PacketFactoryBase, IPlayerDataPacketFactory
    {
        /// <inheritdoc />
        public void SendPlayerData(IPlayerEntity entity, uint playerId, string name, byte jobId, byte level, byte gender, int version, bool online, bool send = true)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(FFPacket.NullId, SnapshotType.QUERY_PLAYER_DATA);

            packet.Write(playerId);
            packet.Write(name);
            packet.Write(jobId);
            packet.Write(level);
            packet.Write(gender);
            packet.Write((byte)0); // padding
            packet.Write(version);
            packet.Write(online);
            packet.Write((byte)0); // padding
            packet.Write((byte)0); // padding
            packet.Write((byte)0); // padding

            if (send)
            {
                SendToPlayer(entity, packet);
            }
        }

        /// <inheritdoc />
        public void SendModifyMode(IPlayerEntity entity)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(entity.Id, SnapshotType.MODIFYMODE);
            packet.Write((uint)entity.PlayerData.Mode);

            SendToVisible(packet, entity, true);
        }
    }
}
