using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions;
using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Server;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker;
using Sylver.HandlerInvoker.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.WorldServer
{
    public sealed class WorldServerUser : FFUserConnection, IWorldServerUser, IGameConnection
    {
        private readonly IHandlerInvoker _handlerInvoker;
        private readonly IPlayerCache _playerCache;
        private readonly IClusterCacheClient _clusterCache;

        public IPlayer Player { get; }

        /// <summary>
        /// Creates a new <see cref="WorldServerUser"/> instance.
        /// </summary>
        public WorldServerUser(ILogger<WorldServerUser> logger, IHandlerInvoker handlerInvoker, IPlayerCache playerCache, IClusterCacheClient clusterCache)
            : base(logger)
        {
            Player = new Player
            {
                Connection = this
            };
            _handlerInvoker = handlerInvoker;
            _playerCache = playerCache;
            _clusterCache = clusterCache;
        }

        public override Task HandleMessageAsync(byte[] packetBuffer)
        {
            uint packetHeaderNumber = 0;

            if (Socket is null)
            {
                Logger.LogTrace($"Skip to handle world packet from null socket. Reason: user is not connected.");
                return Task.CompletedTask;
            }

            PacketType packetType = 0;

            try
            {
                using var packet = new FFPacket(packetBuffer);

                packet.ReadUInt32(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
                packetHeaderNumber = packet.ReadUInt32();
                packetType = (PacketType)packetHeaderNumber;
#if DEBUG
                Logger.LogTrace("[{0}] Received {1} packet from {2}.", Player, packetType, Socket.RemoteEndPoint);
#endif
                _handlerInvoker.Invoke(packetType, Player, packet);
            }
            catch (HandlerActionNotFoundException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    Logger.LogTrace("[{0}] Received an unimplemented World packet {1} (0x{2}) from {3}.", Player, Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                }
                else
                {
                    Logger.LogTrace("[{0}] Received an unknown World packet 0x{1} from {2}.", Player, packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                }
            }
            catch (Exception exception)
            {
                if (packetType == PacketType.WELCOME)
                {
                    Logger.LogError(exception, $"[{Player}] Failed to read incoming packet header. {Environment.NewLine}");
                }
                else
                {
                    Logger.LogError(exception, $"[{Player}] An error occured while handling '{packetType}'{Environment.NewLine}.");

                    if (exception.InnerException != null)
                    {
                        Logger.LogDebug(exception.InnerException?.StackTrace);
                    }
                }
            }

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Player is not null)
                {
                    var cachePlayer = _playerCache.Get(Player.CharacterId);

                    if (cachePlayer is not null)
                    {
                        cachePlayer.Level = Player.Level;
                        cachePlayer.Job = Player.Job.Id;
                        cachePlayer.MessengerStatus = MessengerStatusType.Offline;
                        cachePlayer.IsOnline = false;
                        cachePlayer.Version = 1;

                        _playerCache.Set(cachePlayer);
                        //_clusterCache.DisconnectCharacter(Player.CharacterId);
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
            }

            base.Dispose(disposing);
        }

        protected override void OnConnected()
        {
            Logger.LogInformation("New client connected from {0}.", Socket.RemoteEndPoint);

            using var welcomePacket = new WelcomePacket(SessionId);
            Send(welcomePacket);
        }

        protected override void OnDisconnected()
        {
            Logger.LogInformation("Client disconnected from {0}.", Socket.RemoteEndPoint);
        }
    }
}
