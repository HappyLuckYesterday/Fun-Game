using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Sylver.Network.Client;
using Sylver.Network.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.WorldServer.CoreClient
{
    public class WorldCoreClient : NetClient, IHostedService
    {
        private readonly ILogger<WorldCoreClient> _logger;
        private readonly IWorldServer _worldServer;

        public WorldCoreClient(ILogger<WorldCoreClient> logger, IWorldServer worldServer)
        {
            _logger = logger;
            _worldServer = worldServer;
            ClientConfiguration = new NetClientConfiguration(worldServer.CoreConfiguration.Host,
                worldServer.CoreConfiguration.Port,
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
            packet.WriteString(_worldServer.CoreConfiguration.Password);
            packet.WriteByte((byte)ServerType.World);
            packet.WriteByte((byte)_worldServer.WorldConfiguration.Id);
            packet.WriteByte((byte)_worldServer.WorldConfiguration.ClusterId);
            packet.WriteString(_worldServer.WorldConfiguration.Name);
            packet.WriteString(_worldServer.WorldConfiguration.Host);
            packet.WriteUInt16((ushort)_worldServer.WorldConfiguration.Port);

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
                case CoreAuthenticationResultType.FailedWorldExists:
                    _logger.LogCritical("Unable to authenticate World Core client. Reason: an other World server (with the same id) is already connected.");
                    break;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    _logger.LogCritical("Unable to authenticate World Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    _logger.LogCritical("Unable to authenticate World Core client. Reason: Cannot recognize Core server. You probably have to update all servers.");
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
