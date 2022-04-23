using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Core
{
    public class ClusterCoreClient : LiteClient
    {
        private readonly ILogger<ClusterCoreClient> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public ClusterCoreClient(LiteClientOptions options, ILogger<ClusterCoreClient> logger, IHandlerInvoker handlerInvoker, IServiceProvider serviceProvider = null) 
            : base(options, serviceProvider)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        public override Task HandleMessageAsync(byte[] packetBuffer)
        {
            try
            {
                using var packet = new CorePacket(packetBuffer);
                var packetHeader = (CorePacketType)packet.ReadByte();

                _handlerInvoker.Invoke(packetHeader, this, packet);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing core packet.");
            }

            return Task.CompletedTask;
        }
    }
}
