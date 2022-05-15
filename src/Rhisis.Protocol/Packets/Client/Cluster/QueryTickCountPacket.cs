using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.Cluster
{
    public class QueryTickCountPacket : IPacketDeserializer
    {
        public uint Time { get; private set; }

        public void Deserialize(IFFPacket packet)
        {
            Time = packet.ReadUInt32();
        }
    }
}
