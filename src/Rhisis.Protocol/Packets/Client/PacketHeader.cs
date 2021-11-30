using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client
{
    public class PacketHeader : IPacketDeserializer
    {
        public byte Header { get; private set; }

        public int HashLength { get; private set; }

        public int Length { get; private set; }

        public int HashData { get; private set; }

        public void Deserialize(IFFPacket packet)
        {
            Header = packet.Read<byte>();
            HashLength = packet.Read<int>();
            Length = packet.Read<int>();
            HashData = packet.Read<int>();
        }
    }
}
