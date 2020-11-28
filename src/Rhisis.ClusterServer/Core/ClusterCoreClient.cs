using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
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
        private readonly IClusterServer _clusterServer;

        public ClusterCoreClient(ILogger<ClusterCoreClient> logger, IClusterServer clusterServer)
        {
            _logger = logger;
            _clusterServer = clusterServer;
            ClientConfiguration = new NetClientConfiguration(clusterServer.CoreConfiguration.Host,
                clusterServer.CoreConfiguration.Port, 
                32, new NetClientRetryConfiguration(NetClientRetryOption.Limited, 10));
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            try
            {
                var packetHeader = (CorePacketType)packet.ReadByte();

                if (packetHeader == CorePacketType.Welcome)
                {
                    SendServerInformation();
                }
                else if (packetHeader == CorePacketType.AuthenticationResult)
                {
                    OnAuthenticationResult(packet);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing core packet.");
            }
        }

        private void SendServerInformation()
        {
            using var packet = new NetPacket();

            packet.WriteByte((byte)CorePacketType.Authenticate);
            packet.WriteString(_clusterServer.CoreConfiguration.Password);
            packet.WriteByte((byte)ServerType.Cluster);
            packet.WriteByte((byte)_clusterServer.ClusterConfiguration.Id);
            packet.WriteString(_clusterServer.ClusterConfiguration.Name);
            packet.WriteString(_clusterServer.ClusterConfiguration.Host);
            packet.WriteUInt16((ushort)_clusterServer.ClusterConfiguration.Port);

            Send(packet);
        }

        private void OnAuthenticationResult(INetPacketStream packet)
        {
            var result = (CoreAuthenticationResultType)packet.ReadByte();

            switch (result)
            {
                case CoreAuthenticationResultType.Success:
                    {
                        _logger.LogInformation("Cluster Core client authenticated successfully.");
                        return;
                    }
                case CoreAuthenticationResultType.FailedClusterExists:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: an other Cluster server (with the same id) is already connected.");
                    break;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: Cannot recognize Core server. You probably have to update all servers.");
                    break;
            }

            Environment.Exit((int)result);
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
