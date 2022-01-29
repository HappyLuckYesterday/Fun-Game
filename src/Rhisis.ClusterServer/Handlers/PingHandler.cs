using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client;
using Rhisis.Protocol.Packets.Server;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class PingHandler
    {
        [HandlerAction(PacketType.PING)]
        public void OnPing(IClusterUser user, PingPacket packet)
        {
            if (!packet.IsTimeOut)
            {
                using var pingPacket = new PongPacket(packet.Time);
                user.Send(pingPacket);
            }
        }
    }
}
