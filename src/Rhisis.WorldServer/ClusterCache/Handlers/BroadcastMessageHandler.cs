using Microsoft.Extensions.Logging;
using Rhisis.Protocol.Core;
using Rhisis.Protocol.Messages;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.ClusterCache.Handlers
{
    [Handler]
    internal class BroadcastMessageHandler
    {
        private readonly ILogger<BroadcastMessageHandler> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public BroadcastMessageHandler(ILogger<BroadcastMessageHandler> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        [HandlerAction(CorePacketType.BroadcastMessage)]
        public void OnExecute(IClusterCacheClient client, CorePacket packet)
        {
            string messageText = packet.ReadString();
            CoreMessage message = client.ReadMessage(messageText);

            try
            {
                _handlerInvoker.Invoke(message.Type, this, message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing core packet.");
            }
        }
    }
}
