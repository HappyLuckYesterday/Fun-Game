using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Cache
{
    internal class ClusterCacheUser : LiteServerUser
    {
        private readonly ILogger<ClusterCacheUser> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public WorldChannel Channel { get; } = new WorldChannel();

        public ClusterCacheUser(ILogger<ClusterCacheUser> logger, IHandlerInvoker handlerInvoker)
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
