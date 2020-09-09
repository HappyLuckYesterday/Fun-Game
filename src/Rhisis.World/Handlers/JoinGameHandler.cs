using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Game;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Rhisis.World.Client;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Maps.Regions;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Death;
using Rhisis.World.Systems.PlayerData;
using Rhisis.World.Systems.Teleport;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class JoinGameHandler
    {
        private readonly ILogger<JoinGameHandler> _logger;
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new <see cref="JoinGameHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Database access layer.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public JoinGameHandler(ILogger<JoinGameHandler> logger, IRhisisDatabase database, IGameResources gameResources, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _database = database;
            _gameResources = gameResources;
            _serviceProvider = serviceProvider;
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
                realPlayer.CharacterId = character.Id;
                realPlayer.ModelId = character.Gender == 0 ? 11 : 12;
                realPlayer.Type = WorldObjectType.Mover;
                // TODO: get map and layer
                realPlayer.Position = new Vector3(character.PosX, character.PosY, character.PosZ);
                realPlayer.Angle = character.Angle;
                realPlayer.Size = 100; // TODO: move to constant
                realPlayer.Name = character.Name;
                realPlayer.Level = character.Level;
                realPlayer.ObjectState = ObjectState.OBJSTA_STAND;
                realPlayer.ObjectStateFlags = 0;
                realPlayer.Experience = character.Experience;
                realPlayer.Authority = (AuthorityType)character.User.Authority;
                realPlayer.Mode = ModeType.NONE;
                realPlayer.Slot = character.Slot;
                realPlayer.Appearence = new Rhisis.Game.Abstractions.Components.VisualAppearenceComponent((GenderType)character.Gender)
                {
                    SkinSetId = character.SkinSetId,
                    FaceId = character.FaceId,
                    HairId = character.HairId,
                    HairColor = character.HairColor
                };
                realPlayer.Statistics.AvailablePoints = (ushort)character.StatPoints;
                realPlayer.Statistics.Strength = character.Strength;
                realPlayer.Statistics.Stamina = character.Stamina;
                realPlayer.Statistics.Dexterity = character.Dexterity;
                realPlayer.Statistics.Intelligence = character.Intelligence;

                realPlayer.Health.Hp = character.Hp;
                realPlayer.Health.Mp = Math.Min(0, character.Mp);
                realPlayer.Health.Fp = Math.Min(0, character.Fp);

                if (!_gameResources.Movers.TryGetValue(realPlayer.ModelId, out MoverData moverData))
                {
                    throw new ArgumentException($"Cannot find mover with id '{realPlayer.ModelId}' in game resources.", nameof(realPlayer.ModelId));
                }

                if (!_gameResources.Jobs.TryGetValue((DefineJob.Job)character.JobId, out JobData jobData))
                {
                    throw new ArgumentException($"Cannot find job data with id: '{character.JobId}' in game resources.", nameof(character.JobId));
                }

                realPlayer.Data = moverData;
                realPlayer.Job = jobData;
                realPlayer.Systems = _serviceProvider;

                if (realPlayer.Health.IsDead)
                {
                    // TODO: resurect to lodelight
                }
            }

            var addObjectSnapshot = new AddObjectSnapshot(player);
            // TODO: send

            player.Spawned = true;
            // -----------------------

            //IPlayerEntity player = _playerFactory.CreatePlayer(character);

            //if (player == null)
            //{
            //    _logger.LogError($"Failed to create character for '{character.Name}'.");
            //    return;
            //}

            //player.Connection = serverClient;
            //serverClient.Player = player;

            //if (player.IsDead)
            //{
            //    IMapRevivalRegion revivalRegion = _deathSystem.GetNearestRevivalRegion(player);

            //    if (revivalRegion == null)
            //    {
            //        _logger.LogError($"Cannot resurect player '{player}'; Revival map region not found.");
            //        return;
            //    }

            //    _deathSystem.ApplyRevivalHealthPenality(player);
            //    _deathSystem.ApplyDeathPenality(player, sendToPlayer: false);

            //    IMapInstance map = _mapManager.GetMap(revivalRegion.MapId);

            //    _teleportSystem.ChangePosition(player, map, revivalRegion.X, null, revivalRegion.Z);
            //}

            //_playerDataSystem.CalculateDefense(player);
            //player.Object.CurrentLayer.AddEntity(player);
            //_worldSpawnPacketFactory.SendPlayerSpawn(player);

            //player.Object.Spawned = true;
            //player.PlayerData.LoggedInAt = DateTime.UtcNow;
        }
    }
}
