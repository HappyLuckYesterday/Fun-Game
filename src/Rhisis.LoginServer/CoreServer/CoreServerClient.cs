using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Net.Sockets;

namespace Rhisis.LoginServer.CoreServer
{
    public class CoreServerClient : NetServerClient
    {
        private IServiceProvider _serviceProvider;
        private ILogger<CoreServerClient> _logger;
        private IHandlerInvoker _handlerInvoker;

        public ServerType ServerType => ServerInfo?.ServerType ?? ServerType.Unknown;

        public ServerDescriptor ServerInfo { get; internal set; }

        public CoreServerClient(Socket socketConnection)
            : base(socketConnection)
        {
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
                _logger.LogError(e, "An error occured while processing a core request.");
            }
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetService<ILogger<CoreServerClient>>();
            _handlerInvoker = _serviceProvider.GetRequiredService<IHandlerInvoker>();

            using var packet = new NetPacket();
            packet.WriteByte((byte)CorePacketType.Welcome);
            Send(packet);
        }
    }
}
