using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol.Core;
using Rhisis.Protocol.Core.Servers;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer
{
    public class LoginCoreUser : LiteServerUser
    {
        private readonly ILogger<LoginCoreUser> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public ServerType ServerType => ServerInfo?.ServerType ?? ServerType.Unknown;

        public BaseServer ServerInfo { get; internal set; }

        public LoginCoreUser(ILogger<LoginCoreUser> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        public override Task HandleMessageAsync(ILitePacketStream packet)
        {
            try
            {
                var packetHeader = (LoginCorePacketType)packet.ReadByte();

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
            packet.WriteByte((byte)LoginCorePacketType.Welcome);
            Send(packet);
        }

        protected override void OnDisconnected()
        {
            _logger.LogTrace($"Server '{ServerInfo.Name}' disconnected.");
        }
    }
}
