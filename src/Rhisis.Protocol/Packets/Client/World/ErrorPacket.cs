using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ErrorPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public int Data { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Code = packet.Read<int>();
            Data = packet.Read<int>();
        }
    }
}