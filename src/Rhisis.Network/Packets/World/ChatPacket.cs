using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ChatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; private set; }
        
        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Message = packet.Read<string>();
        }
    }
}
