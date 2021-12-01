using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.LoginServer.Abstractions;
using Rhisis.LoginServer.Packets;
using Rhisis.Protocol;
using Sylver.HandlerInvoker;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.LoginServer
{
    public sealed class LoginClient : LiteServerUser, ILoginClient
    {
        private readonly ILoginServer _server;
        private readonly ILogger<LoginClient> _logger;
        private readonly IHandlerInvoker _handlerInvoker;
        private readonly ILoginPacketFactory _loginPacketFactory;

        public uint SessionId { get; } = RandomHelper.GenerateSessionKey();

        public int UserId { get; private set; }

        public string Username { get; private set; } = "UNKNOWN";

        public bool IsConnected => !string.IsNullOrEmpty(Username);

        /// <summary>
        /// Creates a new <see cref="LoginClient"/> instance.
        /// </summary>
        public LoginClient(ILoginServer server, ILogger<LoginClient> logger, IHandlerInvoker handlerInvoker, ILoginPacketFactory loginPacketFactory)
        {
            _server = server;
            _logger = logger;
            _handlerInvoker = handlerInvoker;
            _loginPacketFactory = loginPacketFactory;
        }

        public void Disconnect() => Disconnect(null);

        public void Disconnect(string reason)
        {
            _server.DisconnectUser(Id);

            if (!string.IsNullOrWhiteSpace(reason))
            {
                _logger.LogInformation($"{Username} disconnected. Reason: {reason}");
            }
        }

        public void SetClientUsername(string username, int userId)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                throw new InvalidOperationException("Client username already set.");
            }

            Username = username;
            UserId = userId;
        }

        public override void Send(ILitePacketStream packet)
        {
            _logger.LogTrace("Send {0} packet to {1}.", (PacketType)BitConverter.ToUInt32(packet.Buffer, 5), Socket.RemoteEndPoint);
            base.Send(packet);
        }

        public override Task HandleMessageAsync(ILitePacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket is null)
            {
                _logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
                return Task.CompletedTask;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();

                _logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, Socket.RemoteEndPoint);
                _handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    _logger.LogTrace("Received an unimplemented Login packet {0} (0x{1}) from {2}.",
                        Enum.GetName(typeof(PacketType), packetHeaderNumber),
                        packetHeaderNumber.ToString("X2"),
                        Socket.RemoteEndPoint);
                }
                else
                {
                    _logger.LogTrace("Received an unknown Login packet 0x{0} from {1}.",
                        packetHeaderNumber.ToString("X2"),
                        Socket.RemoteEndPoint);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a login packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }

            return Task.CompletedTask;
        }

        protected override void OnConnected()
        {
            _logger.LogInformation($"New client connected from {Socket.RemoteEndPoint}.");
            _loginPacketFactory.SendWelcome(this, SessionId);
        }

        protected override void OnDisconnected()
        {
            _logger.LogInformation($"Client '{Username}' disconnected from {Socket.RemoteEndPoint}.");
        }
    }
}