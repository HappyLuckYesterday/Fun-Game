using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ChatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Message = packet.Read<string>();
        }
    }
}
