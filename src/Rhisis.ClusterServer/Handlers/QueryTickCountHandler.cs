using Rhisis.ClusterServer.Client;
using Rhisis.ClusterServer.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets.Cluster;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class QueryTickCountHandler
    {
        private readonly IClusterPacketFactory _clusterPacketFactory;

        public QueryTickCountHandler(IClusterPacketFactory clusterPacketFactory)
        {
            _clusterPacketFactory = clusterPacketFactory;
        }

        [HandlerAction(PacketType.QUERYTICKCOUNT)]
        public void OnQueryTickCount(IClusterClient client, QueryTickCountPacket packet)
        {
            _clusterPacketFactory.SendQueryTickCount(client, packet.Time);
        }
    }
}
