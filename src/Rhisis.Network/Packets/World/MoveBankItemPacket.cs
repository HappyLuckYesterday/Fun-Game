using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
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
        public void Deserialize(ILitePacketStream packet)
        {
            SourceIndex = packet.Read<int>();
            DestinationIndex = packet.Read<int>();
        }
    }
}