using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer
{
    public class CoreUser : LiteServerUser
    {
        private readonly ILogger<CoreUser> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public Cluster Cluster { get; }

        public CoreUser(ILogger<CoreUser> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
            Cluster = new()
            {
                Name = "Unknown"
            };
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
                _logger.LogError(e, "An error occured while processing a core request.");
            }

            return Task.CompletedTask;
        }
        protected override void OnConnected()
        {
            _logger.LogTrace($"New incoming server connection from {Socket.RemoteEndPoint}.");

            using var packet = new CorePacket();
            packet.WriteByte((byte)CorePacketType.Welcome);
            Send(packet);
        }

        protected override void OnDisconnected()
        {
            _logger.LogTrace($"Server '{Cluster.Name}' disconnected.");
        }
    }
}
