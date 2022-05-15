﻿using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Friends
{
    /// <summary>
    /// Provides a data structure representing the add friend request packet.
    /// </summary>
    public class AddFriendRequestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the current player id that is initiating the friend request.
        /// </summary>
        public uint CurrentPlayerId { get; private set; }

        /// <summary>
        /// Gets the target player id that will receive the friend request.
        /// </summary>
        public uint TargetPlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            CurrentPlayerId = packet.ReadUInt32();
            TargetPlayerId = packet.ReadUInt32();
        }
    }
}
