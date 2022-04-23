using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Abstractions.Protocol;
using Rhisis.Abstractions.Server;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Protocol.Core;
using System;
using System.Threading.Tasks;

namespace Rhisis.WorldServer
{
    public class WorldCoreClient : LiteClient
    {
        private readonly ILogger<WorldCoreClient> _logger;
        private readonly IOptions<WorldOptions> _worldOptions;

        public WorldCoreClient(LiteClientOptions options,
            ILogger<WorldCoreClient> logger,
            IOptions<WorldOptions> worldOptions,
            IServiceProvider serviceProvider = null)
            : base(options, serviceProvider)
        {
            _logger = logger;
            _worldOptions = worldOptions;
        }

        public override Task HandleMessageAsync(byte[] packetBuffer)
        {
            try
            {
                using var packet = new CorePacket(packetBuffer);
                var packetHeader = (CorePacketType)packet.ReadByte();

                if (packetHeader == CorePacketType.Welcome)
                {
                    SendServerInformation();
                }
                else if (packetHeader == CorePacketType.AuthenticationResult)
                {
                    OnAuthenticationResult(packet);
                }
                else
                {
                    // Invoke handler
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing core packet.");
            }

            return Task.CompletedTask;
        }

        private void SendServerInformation()
        {
            using var packet = new CorePacket();

            packet.WriteByte((byte)CorePacketType.Authenticate);
            packet.WriteString(_worldOptions.Value.ClusterCache.Password);
            packet.WriteByte((byte)ServerType.World);
            packet.WriteByte((byte)_worldOptions.Value.Id);
            packet.WriteByte((byte)_worldOptions.Value.ClusterId);
            packet.WriteString(_worldOptions.Value.Name);
            packet.WriteString(_worldOptions.Value.Host);
            packet.WriteUInt16((ushort)_worldOptions.Value.Port);

            Send(packet);
        }

        private void OnAuthenticationResult(CorePacket packet)
        {
            var result = (CoreAuthenticationResultType)packet.ReadByte();

            switch (result)
            {
                case CoreAuthenticationResultType.Success:
                    {
                        _logger.LogInformation("World Core client authenticated successfully.");
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
    }
}
