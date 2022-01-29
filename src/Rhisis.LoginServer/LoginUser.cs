using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Server;
using Sylver.HandlerInvoker;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.LoginServer
{
    public sealed class LoginUser : LiteServerUser, ILoginUser
    {
        private const string UnknownUsername = "UNKNOWN";

        private readonly ILoginServer _server;
        private readonly ILogger<LoginUser> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        public uint SessionId { get; } = RandomHelper.GenerateSessionKey();

        public int UserId { get; private set; }

        public string Username { get; private set; } = UnknownUsername;

        public bool IsConnected => !string.IsNullOrEmpty(Username);

        /// <summary>
        /// Creates a new <see cref="LoginUser"/> instance.
        /// </summary>
        public LoginUser(ILoginServer server, ILogger<LoginUser> logger, IHandlerInvoker handlerInvoker)
        {
            _server = server;
            _logger = logger;
            _handlerInvoker = handlerInvoker;
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
            if (!Username.Equals(UnknownUsername, StringComparison.OrdinalIgnoreCase))
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
            PacketType? packetType = null;

            if (Socket is null)
            {
                _logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
                return Task.CompletedTask;
            }

            try
            {
                packetHeaderNumber = packet.ReadUInt32();
                packetType = (PacketType)packetHeaderNumber;

                _logger.LogTrace($"Received {packetType} (0x{packetHeaderNumber:X2}) packet from {Socket.RemoteEndPoint}.");
                _handlerInvoker.Invoke(packetType, this, packet);
            }
            catch (ArgumentException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    string packetName = Enum.GetName(typeof(PacketType), packetHeaderNumber);

                    _logger.LogTrace($"Received an unimplemented Login packet {packetName} (0x{packetHeaderNumber:X2}) from {Socket.RemoteEndPoint}.");
                }
                else
                {
                    _logger.LogTrace($"Received an unknown Login packet 0x{packetHeaderNumber:X2} from {Socket.RemoteEndPoint}.");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occured while handling a login packet.");
            }

            return Task.CompletedTask;
        }

        protected override void OnConnected()
        {
            _logger.LogInformation($"New client connected from {Socket.RemoteEndPoint}.");

            using var welcomePacket = new WelcomePacket(SessionId);
            Send(welcomePacket);
        }

        protected override void OnDisconnected()
        {
            _logger.LogInformation($"Client '{Username}' disconnected from {Socket.RemoteEndPoint}.");
        }
    }
}