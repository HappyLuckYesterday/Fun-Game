using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Cache.Handlers
{
    [Handler]
    public class BroadcastMessageHandler
    {
        private readonly IClusterCacheServer _server;

        public BroadcastMessageHandler(IClusterCacheServer server)
        {
            _server = server;
        }

        [HandlerAction(CorePacketType.BroadcastMessage)]
        public void OnExecute(ClusterCacheUser user, CorePacket packet)
        {
            string message = packet.ReadString();

            using CorePacket messagePacket = new(CorePacketType.BroadcastMessage);
            messagePacket.WriteString(message);

            _server.SendToAll(messagePacket.GetBuffer());
        }
    }
}
