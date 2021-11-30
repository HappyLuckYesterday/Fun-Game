using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class MoveBankItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the source index.
        /// </summary>
        public int SourceIndex { get; private set; }

        /// <summary>
        /// Gets the destination index.
        /// </summary>
        public int DestinationIndex { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            SourceIndex = packet.Read<int>();
            DestinationIndex = packet.Read<int>();
        }
    }
}