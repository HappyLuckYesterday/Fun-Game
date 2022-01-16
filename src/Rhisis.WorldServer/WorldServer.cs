using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Abstractions.Behavior;
using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Features.Chat;
using Rhisis.Abstractions.Map;
using Rhisis.Abstractions.Messaging;
using Rhisis.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Game.Resources.Loaders;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol.Core.Servers;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer
{
    public sealed partial class WorldServer : LiteServer<WorldServerUser>, IWorldServer
    {
        private readonly ILogger<WorldServer> _logger;
        private readonly IOptions<WorldConfiguration> _worldConfiguration;
        private readonly IOptions<CoreConfiguration> _coreConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;
        private readonly IChatCommandManager _chatCommandManager;
        private readonly IRhisisDatabase _database;
        private readonly IRhisisCacheManager _cacheManager;
        private readonly IMessaging _messaging;
        private readonly IHandlerInvoker _handlerInvoker;

        public IEnumerable<IPlayer> ConnectedPlayers => ConnectedUsers.Select(x => x.Player);

        /// <summary>
        /// Creates a new <see cref="WorldServer"/> instance.
        /// </summary>
        public WorldServer(LiteServerOptions serverOptions, 
            ILogger<WorldServer> logger, 
            IOptions<WorldConfiguration> worldConfiguration, 
            IOptions<CoreConfiguration> coreConfiguration,
            IGameResources gameResources, 
            IMapManager mapManager, 
            IBehaviorManager behaviorManager, 
            IChatCommandManager chatCommandManager, 
            IRhisisDatabase database, 
            IRhisisCacheManager cacheManager, 
            IMessaging messaging, 
            IHandlerInvoker handlerInvoker)
            : base(serverOptions)
        {
            _logger = logger;
            _worldConfiguration = worldConfiguration;
            _coreConfiguration = coreConfiguration;
            _gameResources = gameResources;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
            _chatCommandManager = chatCommandManager;
            _database = database;
            _cacheManager = cacheManager;
            _messaging = messaging;
            _handlerInvoker = handlerInvoker;
        }

        /// <inheritdoc />
        protected override void OnBeforeStart()
        {
            if (!_database.IsAlive())
            {
                throw new InvalidProgramException($"Cannot start {nameof(WorldServer)}. Failed to reach database.");
            }

            _gameResources.Load(typeof(DefineLoader),
                typeof(TextLoader),
                typeof(MoverLoader),
                typeof(ItemLoader),
                typeof(DialogLoader),
                typeof(ShopLoader),
                typeof(JobLoader),
                typeof(SkillLoader),
                typeof(ExpTableLoader),
                typeof(PenalityLoader),
                typeof(NpcLoader),
                typeof(QuestLoader));

            _chatCommandManager.Load();
            _behaviorManager.Load();
            _mapManager.Load();

            _messaging.Subscribe<PlayerConnected>(OnPlayerConnectedMessage);
            _messaging.Subscribe<PlayerDisconnected>(OnPlayerDisconnected);
            _messaging.Subscribe<PlayerMessengerStatusUpdate>(OnPlayerStatusUpdateMessage);
            _messaging.Subscribe<PlayerMessengerRemoveFriend>(OnPlayerMessengerRemoveFriendMessage);
            _messaging.Subscribe<PlayerMessengerBlockFriend>(OnPlayerMessengerBlockFriendMessage);
            _messaging.Subscribe<PlayerMessengerMessage>(OnPlayerMessengerMessage);
            _messaging.Subscribe<PlayerCacheUpdate>(OnPlayerCacheUpdateMessage);
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation("'{0}' world server is started and listenening on {1}:{2}.",
                _worldConfiguration.Value.Name, Options.Host, Options.Port);

            IRhisisCache cache = _cacheManager.GetCache(CacheType.ClusterWorldChannels);

            cache.Set(_worldConfiguration.Value.Id.ToString(), new WorldChannel
            {
                ClusterId = _worldConfiguration.Value.ClusterId,
                Host = _worldConfiguration.Value.Host,
                Port = _worldConfiguration.Value.Port,
                Id = _worldConfiguration.Value.Id,
                Name = _worldConfiguration.Value.Name
            });
        }

        protected override void OnBeforeStop()
        {
            IRhisisCache cache = _cacheManager.GetCache(CacheType.ClusterWorldChannels);

            if (cache.Contains(_worldConfiguration.Value.Id.ToString()))
            {
                cache.Delete(_worldConfiguration.Value.Id.ToString());
            }
        }

        public IPlayer GetPlayerEntity(uint id) => ConnectedUsers.FirstOrDefault(x => x.Player.Id == id)?.Player;

        public IPlayer GetPlayerEntity(string name) 
            => ConnectedUsers.FirstOrDefault(x => x.Player.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Player;

        public IPlayer GetPlayerEntityByCharacterId(uint id) 
            => ConnectedUsers.FirstOrDefault(x => x.Player.CharacterId == id)?.Player;

        public uint GetOnlineConnectedPlayerNumber() => (uint)ConnectedUsers.Count();

        private void OnPlayerConnectedMessage(PlayerConnected playerConnectedMessage)
        {
            OnPlayerStatusUpdateMessage(new PlayerMessengerStatusUpdate(playerConnectedMessage.Id, playerConnectedMessage.Status));

            // TODO: notify core server that a player has connected.
        }

        private void OnPlayerDisconnected(PlayerDisconnected playerDisconnectedMessage)
        {
            OnPlayerStatusUpdateMessage(new PlayerMessengerStatusUpdate(playerDisconnectedMessage.Id, MessengerStatusType.Offline));

            // TODO: notify core server that a player has disconnected.
        }

        private void OnPlayerStatusUpdateMessage(PlayerMessengerStatusUpdate playerMessengerStatusUpdate)
        {
            _handlerInvoker.Invoke(typeof(PlayerMessengerStatusUpdate), playerMessengerStatusUpdate);
        }

        private void OnPlayerMessengerRemoveFriendMessage(PlayerMessengerRemoveFriend friendRemovalMessage)
        {
            _handlerInvoker.Invoke(typeof(PlayerMessengerRemoveFriend), friendRemovalMessage);
        }

        private void OnPlayerMessengerBlockFriendMessage(PlayerMessengerBlockFriend friendBlockedMessage)
        {
            _handlerInvoker.Invoke(typeof(PlayerMessengerBlockFriend), friendBlockedMessage);
        }

        private void OnPlayerMessengerMessage(PlayerMessengerMessage message)
        {
            _handlerInvoker.Invoke(typeof(PlayerMessengerMessage), message);
        }

        private void OnPlayerCacheUpdateMessage(PlayerCacheUpdate message)
        {
            _handlerInvoker.Invoke(typeof(PlayerCacheUpdate), message);
        }
    }
}
