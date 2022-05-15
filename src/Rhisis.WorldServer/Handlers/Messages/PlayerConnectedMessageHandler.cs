using Rhisis.Protocol.Messages.Cluster;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.Messages
{
    [Handler]
    public class PlayerConnectedMessageHandler
    {
        private readonly IWorldServer _worldSever;

        public PlayerConnectedMessageHandler(IWorldServer worldSever)
        {
            _worldSever = worldSever;
        }

        [HandlerAction(typeof(PlayerConnectedMessage))]
        public void OnExecute(IClusterCacheClient _, PlayerConnectedMessage message)
        {
        }
    }
}
