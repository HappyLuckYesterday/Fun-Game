using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ActMsgPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        public uint Message { get; private set; }

        /// <summary>
        /// Gets the first parameter.
        /// </summary>
        public int Parameter1 { get; private set; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Message = packet.Read<uint>();
            Parameter1 = packet.Read<int>();
            Parameter2 = packet.Read<int>();
        }
    }
}