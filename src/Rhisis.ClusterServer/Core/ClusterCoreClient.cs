using LiteNetwork.Client;
using LiteNetwork.Protocol.Abstractions;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
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

        public override Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            try
            {
                var packetHeader = (CorePacketType)incomingPacketStream.ReadByte();

                _handlerInvoker.Invoke(packetHeader, this, incomingPacketStream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing core packet.");
            }


            return Task.CompletedTask;
        }
    }
}
