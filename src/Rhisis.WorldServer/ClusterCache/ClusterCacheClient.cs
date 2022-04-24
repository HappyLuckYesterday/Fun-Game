using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Protocol.Core;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.WorldServer.ClusterCache
{
    internal class ClusterCacheClient : LiteClient, IClusterCacheClient
    {
        private readonly ILogger<ClusterCacheClient> _logger;
        private readonly IOptions<WorldOptions> _worldOptions;
        private readonly IHandlerInvoker _handlerInvoker;
        private readonly IWorldServer _worldServer;

        public ClusterCacheClient(LiteClientOptions options, 
            IServiceProvider serviceProvider, 
            ILogger<ClusterCacheClient> logger, 
            IOptions<WorldOptions> worldOptions, 
            IHandlerInvoker handlerInvoker,
            IWorldServer worldServer)
            : base(options, serviceProvider)
        {
            _logger = logger;
            _worldOptions = worldOptions;
            _handlerInvoker = handlerInvoker;
            _worldServer = worldServer;
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

        protected override void OnConnected()
        {
            _logger.LogTrace("Connected to Cluster Cache server.");
        }

        protected override void OnDisconnected()
        {
            _logger.LogTrace("Disconnected from Cluster Cache server.");
        }

        public void AuthenticateWorldServer()
        {
            using var packet = new CorePacket();

            packet.WriteByte((byte)CorePacketType.AuthenticationRequest);
            packet.WriteString(_worldOptions.Value.ClusterCache.Password);
            packet.WriteByte((byte)_worldOptions.Value.Id);
            packet.WriteString(_worldOptions.Value.Name);
            packet.WriteString(_worldOptions.Value.Host);
            packet.WriteUInt16((ushort)_worldOptions.Value.Port);
            packet.WriteInt32(_worldServer.ConnectedPlayers.Count());
            packet.WriteInt32(_worldOptions.Value.MaximumUsers);

            Send(packet);
        }
    }
}
