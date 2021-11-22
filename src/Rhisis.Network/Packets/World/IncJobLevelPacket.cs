using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class IncJobLevelPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Id = packet.Read<byte>();
        }
    }
}