using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
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
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker;
using Sylver.HandlerInvoker.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.WorldServer
{
    public sealed class WorldServerUser : LiteServerUser, IWorldServerUser, IGameConnection
    {
        private readonly ILogger<WorldServerUser> _logger;
        private readonly IHandlerInvoker _handlerInvoker;
        private readonly IPlayerCache _playerCache;
        private readonly IMessaging _messaging;

        public uint SessionId { get; } = RandomHelper.GenerateSessionKey();

        public IPlayer Player { get; }

        /// <summary>
        /// Creates a new <see cref="WorldServerUser"/> instance.
        /// </summary>
        public WorldServerUser(ILogger<WorldServerUser> logger, IHandlerInvoker handlerInvoker, IPlayerCache playerCache, IMessaging messaging)
        {
            Player = new Player
            {
                Connection = this
            };
            _logger = logger;
            _handlerInvoker = handlerInvoker;
            _playerCache = playerCache;
            _messaging = messaging;
        }

        /// <summary>
        /// Initialize the client and send welcome packet.
        /// </summary>
        public void Initialize(IServiceProvider serviceProvider)
        {
            using var welcomePacket = new WelcomePacket(SessionId);
            Send(welcomePacket);
        }

        /// <inheritdoc />
        public override Task HandleMessageAsync(ILitePacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket is null)
            {
                _logger.LogTrace($"Skip to handle world packet from null socket. Reason: user is not connected.");
                return Task.CompletedTask;
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

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (Player is not null)
            {
                var cachePlayer = _playerCache.GetCachedPlayer(Player.CharacterId);

                if (cachePlayer is not null)
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

                if (Player.MapLayer is not null)
                {
                    Player.MapLayer.RemovePlayer(Player);
                }

                var initializers = Player.Systems.GetService<IEnumerable<IPlayerInitializer>>();

                foreach (IPlayerInitializer initializer in initializers)
                {
                    initializer.Save(Player);
                }
            }

            base.Dispose();
        }

        protected override void OnConnected()
        {
            _logger.LogInformation("New client connected from {0}.", Socket.RemoteEndPoint);
        }

        protected override void OnDisconnected()
        {
            _logger.LogInformation("Client disconnected from {0}.", Socket.RemoteEndPoint);
        }
    }
}
