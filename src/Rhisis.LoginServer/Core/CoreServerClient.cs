using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol.Core;
using Rhisis.Protocol.Core.Servers;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer.Core
{
    public class CoreServerClient : LiteServerUser
    {
        private readonly ILogger<CoreServerClient> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public ServerType ServerType => ServerInfo?.ServerType ?? ServerType.Unknown;

        public ServerDescriptor ServerInfo { get; internal set; }

        public CoreServerClient(ILogger<CoreServerClient> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        public override Task HandleMessageAsync(ILitePacketStream packet)
        {
            try
            {
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

            using var packet = new LitePacket();
            packet.WriteByte((byte)CorePacketType.Welcome);
            Send(packet);
        }

        protected override void OnDisconnected()
        {
            _logger.LogTrace($"Server '{ServerInfo.Name}' disconnected.");
        }
    }
}
