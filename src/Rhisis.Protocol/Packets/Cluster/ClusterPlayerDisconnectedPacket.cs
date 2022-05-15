using Rhisis.Protocol.Core;

namespace Rhisis.Protocol.Packets.Cluster
{
    public class ClusterPlayerDisconnectedPacket : CorePacket
    {
        public ClusterPlayerDisconnectedPacket(int characterId)
            : base(CorePacketType.PlayerDisconnectedFromChannel)
        {
            WriteInt32(characterId);
        }
    }
}
