using Rhisis.Abstractions.Protocol;

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
            Header = packet.ReadByte();
            HashLength = packet.ReadInt32();
            Length = packet.ReadInt32();
            HashData = packet.ReadInt32();
        }
    }
}
