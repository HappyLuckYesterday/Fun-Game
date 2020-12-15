using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker;
using Sylver.Network.Client;
using Sylver.Network.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Core
{
    public class ClusterCoreClient : NetClient, IHostedService
    {
        private readonly ILogger<ClusterCoreClient> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public ClusterCoreClient(ILogger<ClusterCoreClient> logger, IClusterServer clusterServer, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
            ClientConfiguration = new NetClientConfiguration(clusterServer.CoreConfiguration.Host,
                clusterServer.CoreConfiguration.Port, 
                32, new NetClientRetryConfiguration(NetClientRetryOption.Limited, 10));
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            try
            {
                var packetHeader = (CorePacketType)packet.ReadByte();

                _handlerInvoker.Invoke(packetHeader, this, packet);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing core packet.");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Connect();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Disconnect();

            return Task.CompletedTask;
        }
    }
}
