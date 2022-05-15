using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class IncJobLevelPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Id = packet.ReadByte();
        }
    }
}