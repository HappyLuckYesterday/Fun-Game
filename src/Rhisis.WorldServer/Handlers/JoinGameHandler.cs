using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.DependencyInjection.Extensions;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Components;
using Rhisis.Game.Entities;
using Rhisis.Game.Features;
using Rhisis.Game.Protocol.Packets;
using Rhisis.Game.Protocol.Snapshots.Friends;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class JoinGameHandler
    {
        private readonly ILogger<JoinGameHandler> _logger;
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<WorldConfiguration> _configuration;
        private readonly IPlayerCache _playerCache;

        /// <summary>
        /// Creates a new <see cref="JoinGameHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Database access layer.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="mapManager">Map manager.</param>
        /// <param name="behaviorManager">Behavior manager.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public JoinGameHandler(ILogger<JoinGameHandler> logger, IRhisisDatabase database, 
            IGameResources gameResources, IMapManager mapManager, 
            IBehaviorManager behaviorManager, IServiceProvider serviceProvider,
            IOptions<WorldConfiguration> configuration, IPlayerCache playerCache)
        {
            _logger = logger;
            _database = database;
            _gameResources = gameResources;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _playerCache = playerCache;
        }

        /// <summary>
        /// Prepares the player to join the world.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming join packet.</param>
        [HandlerAction(PacketType.JOIN)]
        public void OnJoin(IPlayer player, JoinPacket packet)
        {
            DbCharacter character = _database.Characters.Include(x => x.User).FirstOrDefault(x => x.Id == packet.PlayerId);

            if (character == null)
            {
                throw new ArgumentNullException($"Cannot find character with id: {packet.PlayerId}.");
            }

            if (character.IsDeleted)
            {
                throw new InvalidOperationException($"Cannot connect with character '{character.Name}' for user '{character.User.Username}'. Reason: character is deleted.");
            }

            if (character.User.Authority <= 0)
            {
                _logger.LogWarning($"Cannot connect with '{character.Name}'. Reason: User {character.User.Username} is banned.");
                // TODO: send error to client
                return;
            }

            if (player is Player realPlayer)
            {
                // TODO: move this to constants somewhere
                int playerModelId = character.Gender == 0 ? 11 : 12;

                if (!_gameResources.Movers.TryGetValue(playerModelId, out MoverData moverData))
                {
                    throw new ArgumentException($"Cannot find mover with id '{realPlayer.ModelId}' in game resources.", nameof(realPlayer.ModelId));
                }

                if (!_gameResources.Jobs.TryGetValue((DefineJob.Job)character.JobId, out JobData jobData))
                {
                    throw new ArgumentException($"Cannot find job data with id: '{character.JobId}' in game resources.", nameof(character.JobId));
                }

                realPlayer.Systems = _serviceProvider;
                realPlayer.Data = moverData;
                realPlayer.Job = jobData;
                realPlayer.Behavior = _behaviorManager.GetDefaultBehavior(BehaviorType.Player, realPlayer);
                realPlayer.CharacterId = character.Id;
                realPlayer.ModelId = playerModelId;
                realPlayer.Type = WorldObjectType.Mover;
                realPlayer.MapLayer = _mapManager.GetMap(character.MapId)?.GetMapLayer(character.MapLayerId) ?? throw new InvalidOperationException($"Cannot create player on map with id: {character.Id}.");
                realPlayer.Position = new Vector3(character.PosX, character.PosY, character.PosZ);
                realPlayer.Angle = character.Angle;
                realPlayer.Size = GameConstants.DefaultObjectSize;
                realPlayer.Name = character.Name;
                realPlayer.Level = character.Level;
                realPlayer.DeathLevel = character.Level;
                realPlayer.ObjectState = ObjectState.OBJSTA_STAND;
                realPlayer.ObjectStateFlags = 0;
                realPlayer.Authority = (AuthorityType)character.User.Authority;
                realPlayer.Mode = ModeType.NONE;
                realPlayer.Slot = character.Slot;
                
                realPlayer.Appearence = new HumanVisualAppearenceComponent((GenderType)character.Gender)
                {
                    SkinSetId = character.SkinSetId,
                    FaceId = character.FaceId,
                    HairId = character.HairId,
                    HairColor = character.HairColor
                };

                realPlayer.Gold = _serviceProvider.CreateInstance<Gold>(realPlayer, character.Gold);
                realPlayer.Experience = _serviceProvider.CreateInstance<Experience>(realPlayer, character.Experience);
                realPlayer.Inventory = _serviceProvider.CreateInstance<Rhisis.Game.Features.Inventory>(realPlayer);
                realPlayer.Chat = _serviceProvider.CreateInstance<Rhisis.Game.Features.Chat.Chat>(realPlayer);
                realPlayer.Attributes = _serviceProvider.CreateInstance<Attributes>(realPlayer);
                realPlayer.Battle = _serviceProvider.CreateInstance<Rhisis.Game.Features.Battle>(realPlayer);
                realPlayer.Quests = _serviceProvider.CreateInstance<QuestDiary>(realPlayer);
                realPlayer.SkillTree = _serviceProvider.CreateInstance<SkillTree>(realPlayer, (ushort)character.SkillPoints);
                realPlayer.Taskbar = _serviceProvider.CreateInstance<Taskbar>();
                realPlayer.Projectiles = _serviceProvider.CreateInstance<Projectiles>();
                realPlayer.Delayer = _serviceProvider.CreateInstance<Delayer>();
                realPlayer.Buffs = _serviceProvider.CreateInstance<Buffs>(realPlayer);
                realPlayer.Messenger = _serviceProvider.CreateInstance<Messenger>(realPlayer, _configuration.Value.Id, _configuration.Value.Messenger.MaximumFriends);

                IEnumerable<IPlayerInitializer> playerInitializers = _serviceProvider.GetRequiredService<IEnumerable<IPlayerInitializer>>();

                foreach (IPlayerInitializer initializer in playerInitializers)
                {
                    initializer.Load(realPlayer);
                }

                realPlayer.Statistics = _serviceProvider.CreateInstance<PlayerStatistics>(realPlayer);
                realPlayer.Statistics.AvailablePoints = (ushort)character.StatPoints;
                realPlayer.Statistics.Strength = character.Strength;
                realPlayer.Statistics.Stamina = character.Stamina;
                realPlayer.Statistics.Dexterity = character.Dexterity;
                realPlayer.Statistics.Intelligence = character.Intelligence;

                realPlayer.Health = _serviceProvider.CreateInstance<Health>(realPlayer);
                realPlayer.Health.Hp = character.Hp;
                realPlayer.Health.Mp = character.Mp;
                realPlayer.Health.Fp = character.Fp;

                realPlayer.Defense = _serviceProvider.CreateInstance<Defense>(realPlayer);
                realPlayer.Defense.Update();

                if (realPlayer.Health.IsDead)
                {
                    realPlayer.Experience.ApplyDeathPenality(true);
                    realPlayer.Health.ApplyDeathRecovery(true);

                    IMapRevivalRegion revivalRegion = realPlayer.Map.GetNearRevivalRegion(realPlayer.Position);

                    if (revivalRegion == null)
                    {
                        throw new InvalidOperationException("Cannot find nearest revival region.");
                    }

                    if (realPlayer.Map.Id != revivalRegion.MapId)
                    {
                        IMap revivalMap = _mapManager.GetMap(revivalRegion.MapId);

                        if (revivalMap == null)
                        {
                            throw new InvalidOperationException($"Failed to find map with id: {revivalMap.Id}'.");
                        }

                        revivalRegion = revivalMap.GetRevivalRegion(revivalRegion.Key);
                    }

                    realPlayer.MapLayer = _mapManager.GetMap(revivalRegion.MapId).GetDefaultMapLayer();
                    realPlayer.Position.Copy(revivalRegion.RevivalPosition);
                }

                realPlayer.LoggedInAt = DateTime.UtcNow;
            }

            var cachedPlayer = new CachedPlayer(player.CharacterId, _configuration.Value.Id, player.Name, player.Appearence.Gender)
            {
                Level = player.Level,
                Job = player.Job.Id,
                Version = 1,
                IsOnline = true
            };

            _playerCache.SetCachedPlayer(cachedPlayer);

            using (var joinPacket = new JoinCompletePacket())
            {
                joinPacket.AddSnapshots(
                    new EnvironmentAllSnapshot(player, SeasonType.None), // TODO: get the season id using current weather time.
                    new WorldReadInfoSnapshot(player),
                    new AddObjectSnapshot(player),
                    new TaskbarSnapshot(player),
                    new QueryPlayerDataSnapshot(cachedPlayer),
                    new AddFriendGameJoinSnapshot(player)
                );

                player.Connection.Send(joinPacket);
            }

            player.MapLayer.AddPlayer(player);
            player.Spawned = true;
        }
    }
}
