using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.Cluster
{
    public class QueryTickCountPacket : IPacketDeserializer
    {
        public uint Time { get; private set; }

        public void Deserialize(ILitePacketStream packet)
        {
            Time = packet.Read<uint>();
        }
    }
}
