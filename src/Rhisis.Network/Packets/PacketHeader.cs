using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets
{
    public class PacketHeader : IPacketDeserializer
    {
        public byte Header { get; private set; }

        public int HashLength { get; private set; }

        public int Length { get; private set; }

        public int HashData { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Header = packet.Read<byte>();
            HashLength = packet.Read<int>();
            Length = packet.Read<int>();
            HashData = packet.Read<int>();
        }
    }
}
