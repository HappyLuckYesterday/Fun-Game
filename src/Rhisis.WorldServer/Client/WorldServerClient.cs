using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Game.Protocol.Packets;
using Rhisis.Network;
using Sylver.HandlerInvoker;
using Sylver.HandlerInvoker.Exceptions;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Rhisis.WorldServer.Client
{
    public sealed class WorldServerClient : NetServerClient, IWorldServerClient, IGameConnection
    {
        private ILogger<WorldServerClient> _logger;
        private IHandlerInvoker _handlerInvoker;
        private IPlayerCache _playerCache;
        private IMessaging _messaging;

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
        public void Initialize(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<WorldServerClient>>();
            _handlerInvoker = serviceProvider.GetRequiredService<IHandlerInvoker>();
            _playerCache = serviceProvider.GetRequiredService<IPlayerCache>() ?? throw new InvalidOperationException($"Failed to get player cache.");
            _messaging = serviceProvider.GetRequiredService<IMessaging>() ?? throw new InvalidOperationException($"Failed to get messaging system.");

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
                _logger.LogTrace("[{0}] Received {1} packet from {2}.", Player, packetType, Socket.RemoteEndPoint);
#endif
                _handlerInvoker.Invoke(packetType, Player, packet);
            }
            catch (HandlerActionNotFoundException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    _logger.LogTrace("[{0}] Received an unimplemented World packet {1} (0x{2}) from {3}.", Player, Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                }
                else
                {
                    _logger.LogTrace("[{0}] Received an unknown World packet 0x{1} from {2}.", Player, packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
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
                    var cachePlayer = _playerCache.GetCachedPlayer(Player.CharacterId);

                    if (cachePlayer != null)
                    {
                        cachePlayer.Level = Player.Level;
                        cachePlayer.Job = Player.Job.Id;
                        cachePlayer.MessengerStatus = MessengerStatusType.Offline;
                        cachePlayer.IsOnline = false;
                        cachePlayer.Version = 1;

                        _playerCache.SetCachedPlayer(cachePlayer);
                        _messaging.Publish(new PlayerDisconnected(Player.CharacterId));
                    }

                    Player.Spawned = false;

                    if (Player.MapLayer != null)
                    {
                        Player.MapLayer.RemovePlayer(Player);
                    }

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
