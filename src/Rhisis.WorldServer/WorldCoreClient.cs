using LiteNetwork.Client;
using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Network.Core;
using System;
using System.Threading.Tasks;

namespace Rhisis.WorldServer
{
    public class WorldCoreClient : LiteClient
    {
        private readonly ILogger<WorldCoreClient> _logger;
        private readonly IOptions<WorldConfiguration> _serverOptions;
        private readonly IOptions<CoreConfiguration> _coreOptions;

        public WorldCoreClient(LiteClientOptions options,
            ILogger<WorldCoreClient> logger,
            IOptions<WorldConfiguration> serverOptions,
            IOptions<CoreConfiguration> coreOptions,
            IServiceProvider serviceProvider = null)
            : base(options, serviceProvider)
        {
            _logger = logger;
            _serverOptions = serverOptions;
            _coreOptions = coreOptions;
        }

        public override Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            try
            {
                var packetHeader = (CorePacketType)incomingPacketStream.ReadByte();

                if (packetHeader == CorePacketType.Welcome)
                {
                    SendServerInformation();
                }
                else if (packetHeader == CorePacketType.AuthenticationResult)
                {
                    OnAuthenticationResult(incomingPacketStream);
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
            using var packet = new LitePacket();

            packet.WriteByte((byte)CorePacketType.Authenticate);
            packet.WriteString(_coreOptions.Value.Password);
            packet.WriteByte((byte)ServerType.World);
            packet.WriteByte((byte)_serverOptions.Value.Id);
            packet.WriteByte((byte)_serverOptions.Value.ClusterId);
            packet.WriteString(_serverOptions.Value.Name);
            packet.WriteString(_serverOptions.Value.Host);
            packet.WriteUInt16((ushort)_serverOptions.Value.Port);

            Send(packet);
        }

        private void OnAuthenticationResult(ILitePacketStream packet)
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
