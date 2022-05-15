using Rhisis.Protocol.Core;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.ClusterCache.Handlers
{
    [Handler]
    internal class WelcomeHandler
    {
        [HandlerAction(CorePacketType.Welcome)]
        public void OnExecute(IClusterCacheClient client, CorePacket _)
        {
            client.AuthenticateWorldServer();
        }
    }
}
