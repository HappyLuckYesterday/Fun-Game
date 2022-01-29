using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Helpers;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Server;
using Sylver.HandlerInvoker;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer
{
    public sealed class ClusterUser : LiteServerUser, IClusterUser
    {
        private readonly ILogger<ClusterUser> _logger;
        private readonly IHandlerInvoker _handlerInvoker;
        private readonly IClusterServer _server;

        public uint SessionId { get; } = RandomHelper.GenerateSessionKey();

        public int UserId { get; set; }

        public string Username { get; set; }

        public int LoginProtectValue { get; set; } = new Random().Next(0, 1000);

        /// <summary>
        /// Creates a new <see cref="ClusterUser"/> instance.
        /// </summary>
        /// <param name="server">Cluster server.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        public ClusterUser(IClusterServer server, ILogger<ClusterUser> logger, IHandlerInvoker handlerInvoker)
        {
            _server = server;
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        public void Disconnect() => _server.DisconnectUser(Id);

        /// <summary>
        /// Send a packet to the client.
        /// </summary>
        /// <param name="packet"></param>
        public override void Send(ILitePacketStream packet)
        {
            _logger.LogTrace("Send {0} packet to {1}.",
                    (PacketType)BitConverter.ToUInt32(packet.Buffer, 5),
                    Socket.RemoteEndPoint);

            base.Send(packet);
        }

        /// <summary>
        /// Handle the incoming mesages.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override Task HandleMessageAsync(ILitePacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket is null)
            {
                _logger.LogTrace("Skip to handle cluster packet from null socket. Reason: client is not connected.");
                return Task.CompletedTask;
            }

            try
            {
                packet.ReadUInt32(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
                packetHeaderNumber = packet.ReadUInt32();

#if DEBUG
                _logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, Socket.RemoteEndPoint);
#endif
                _handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentNullException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    _logger.LogTrace("Received an unimplemented Cluster packet {0} (0x{1}) from {2}.",
                        Enum.GetName(typeof(PacketType), packetHeaderNumber),
                        packetHeaderNumber.ToString("X4"),
                        Socket.RemoteEndPoint);
                }
                else
                {
                    _logger.LogTrace("[SECURITY] Received an unknown Cluster packet 0x{0} from {1}.",
                        packetHeaderNumber.ToString("X4"),
                        Socket.RemoteEndPoint);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a cluster packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }

            return Task.CompletedTask;
        }

        protected override void OnConnected()
        {
            _logger.LogInformation($"New client connected to cluster server from {Socket.RemoteEndPoint}.");

            using var welcomePacket = new WelcomePacket(SessionId);
            Send(welcomePacket);
        }

        protected override void OnDisconnected()
        {
            _logger.LogInformation($"Client disconnected from {Socket?.RemoteEndPoint.ToString() ?? "unknown location"}.");
        }
    }
}
