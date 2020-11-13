using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets;
using Rhisis.Network;
using Sylver.HandlerInvoker;
using Sylver.HandlerInvoker.Exceptions;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Rhisis.World.Client
{
    public sealed class WorldServerClient : NetServerClient, IWorldServerClient, IGameConnection
    {
        private ILogger<WorldServerClient> _logger;
        private IHandlerInvoker _handlerInvoker;

        public uint SessionId { get; }

        public IPlayer Player { get; }

        /// <summary>
        /// Creates a new <see cref="WorldServerClient"/> instance.
        /// </summary>
        /// <param name="socketConnection">Socket connectin.</param>
        public WorldServerClient(Socket socketConnection)
            : base(socketConnection)
        {
            SessionId = RandomHelper.GenerateSessionKey();
            Player = new Player
            {
                Connection = this
            };
        }

        /// <summary>
        /// Initialize the client and send welcome packet.
        /// </summary>
        public void Initialize(ILogger<WorldServerClient> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;

            using var welcomePacket = new WelcomePacket(SessionId);
            Send(welcomePacket);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogTrace($"Skip to handle world packet from null socket. Reason: {nameof(WorldServerClient)} is not connected.");
                return;
            }

            PacketType packetType = 0;

            try
            {
                packet.Read<uint>(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
                packetHeaderNumber = packet.Read<uint>();
                packetType = (PacketType)packetHeaderNumber;
#if DEBUG
                _logger.LogTrace($"[{Player}] Received {0} packet from {1}.", packetType, Socket.RemoteEndPoint);
#endif
                _handlerInvoker.Invoke(packetType, Player, packet);
            }
            catch (HandlerActionNotFoundException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    _logger.LogTrace($"[{Player}] Received an unimplemented World packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                }
                else
                {
                    _logger.LogTrace($"[{Player}] Received an unknown World packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                }
            }
            catch (Exception exception)
            {
                if (packetType == PacketType.WELCOME)
                {
                    _logger.LogError(exception, $"[{Player}] Failed to read incoming packet header. {Environment.NewLine}");
                }
                else
                {
                    _logger.LogError(exception, $"[{Player}] An error occured while handling '{packetType}'{Environment.NewLine}.");
                    
                    if (exception.InnerException != null)
                    {
                        _logger.LogDebug(exception.InnerException?.StackTrace);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Player != null)
                {
                    Player.Spawned = false;
                    Player.MapLayer.RemovePlayer(Player);

                    var initializers = Player.Systems.GetService<IEnumerable<IPlayerInitializer>>();

                    foreach (IPlayerInitializer initializer in initializers)
                    {
                        initializer.Save(Player);
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}
