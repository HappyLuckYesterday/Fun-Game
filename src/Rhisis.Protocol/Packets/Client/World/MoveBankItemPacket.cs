using Rhisis.Abstractions.Protocol;

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
            SourceIndex = packet.ReadInt32();
            DestinationIndex = packet.ReadInt32();
        }
    }
}