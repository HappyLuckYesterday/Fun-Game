using Rhisis.ClusterServer.Client;
using Rhisis.ClusterServer.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class PingHandler
    {
        private readonly IClusterPacketFactory _clusterPacketFactory;

        public PingHandler(IClusterPacketFactory clusterPacketFactory)
        {
            _clusterPacketFactory = clusterPacketFactory;
        }

        [HandlerAction(PacketType.PING)]
        public void OnPing(IClusterClient client, PingPacket pingPacket)
        {
            if (!pingPacket.IsTimeOut)
            {
                _clusterPacketFactory.SendPong(client, pingPacket.Time);
            }
        }
    }
}
