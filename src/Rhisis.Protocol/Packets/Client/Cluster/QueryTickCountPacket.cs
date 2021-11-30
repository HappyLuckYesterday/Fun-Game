using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.Cluster
{
    public class QueryTickCountPacket : IPacketDeserializer
    {
        public uint Time { get; private set; }

        public void Deserialize(IFFPacket packet)
        {
            Time = packet.Read<uint>();
        }
    }
}
