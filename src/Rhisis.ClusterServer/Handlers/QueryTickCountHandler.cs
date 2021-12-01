using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Packets;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
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
        public void OnQueryTickCount(IClusterUser client, QueryTickCountPacket packet)
        {
            _clusterPacketFactory.SendQueryTickCount(client, packet.Time);
        }
    }
}
