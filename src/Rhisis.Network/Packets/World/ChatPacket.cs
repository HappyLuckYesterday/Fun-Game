﻿using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ChatPacket"/> structure.
    /// </summary>
    public class ChatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; private set; }
        
        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Message = packet.Read<string>();
        }
    }
}
