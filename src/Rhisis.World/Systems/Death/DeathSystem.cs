﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Common.Formulas;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Maps.Regions;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Teleport;

namespace Rhisis.World.Systems.Death
{
    [Injectable]
    public sealed class DeathSystem : IDeathSystem
    {
        private readonly ILogger<DeathSystem> _logger;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly ITeleportSystem _teleportSystem;
        private readonly IPlayerPacketFactory _playerPacketFactory;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public DeathSystem(ILogger<DeathSystem> logger, IOptions<WorldConfiguration> worldConfiguration, IGameResources gameResources, IMapManager mapManager, ITeleportSystem teleportSystem, IPlayerPacketFactory playerPacketFactory, IMoverPacketFactory moverPacketFactory)
        {
            this._logger = logger;
            this._worldConfiguration = worldConfiguration.Value;
            this._gameResources = gameResources;
            this._mapManager = mapManager;
            this._teleportSystem = teleportSystem;
            this._playerPacketFactory = playerPacketFactory;
            this._moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public void ResurectLodelight(IPlayerEntity player)
        {
            IMapRevivalRegion revivalRegion = player.Object.CurrentMap.GetNearRevivalRegion(player.Object.Position);

            if (revivalRegion == null)
            {
                this._logger.LogError($"Cannot find any revival region for map '{player.Object.CurrentMap.Name}'.");
                return;
            }

            decimal recoveryRate = this._gameResources.Penalities.GetRevivalPenality(player.Object.Level) / 100;
            var jobData = player.PlayerData.JobData;

            int strength = player.Attributes[DefineAttributes.STR];
            int stamina = player.Attributes[DefineAttributes.STA];
            int dexterity = player.Attributes[DefineAttributes.DEX];
            int intelligence = player.Attributes[DefineAttributes.INT];

            player.Health.Hp = (int)(HealthFormulas.GetMaxOriginHp(player.Object.Level, stamina, jobData.MaxHpFactor) * recoveryRate);
            player.Health.Mp = (int)(HealthFormulas.GetMaxOriginMp(player.Object.Level, intelligence, jobData.MaxMpFactor, true) * recoveryRate);
            player.Health.Fp = (int)(HealthFormulas.GetMaxOriginFp(player.Object.Level, stamina, dexterity, strength, jobData.MaxFpFactor, true) * recoveryRate);

            if (player.Object.MapId != revivalRegion.MapId)
            {
                IMapInstance revivalMap = this._mapManager.GetMap(revivalRegion.MapId);

                if (revivalMap == null)
                {
                    this._logger.LogError($"Cannot find revival map with id '{revivalRegion.MapId}'.");
                    player.Connection.Server.DisconnectClient(player.Connection.Id);
                    return;
                }

                revivalRegion = revivalMap.GetRevivalRegion(revivalRegion.Key);
            }

            this._teleportSystem.Teleport(player, revivalRegion.MapId, revivalRegion.RevivalPosition.X, null, revivalRegion.RevivalPosition.Z);

            this._moverPacketFactory.SendMotion(player, ObjectMessageType.OBJMSG_ACC_STOP | ObjectMessageType.OBJMSG_STOP_TURN | ObjectMessageType.OBJMSG_STAND);
            this._playerPacketFactory.SendPlayerRevival(player);
            this._moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.HP, player.Health.Hp);
            this._moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.MP, player.Health.Mp);
            this._moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.FP, player.Health.Fp);

            this.ProcessDeathPenality(player);
        }

        /// <summary>
        /// Applies death penality if enabled.
        /// </summary>
        /// <param name="player">Death player entity.</param>
        private void ProcessDeathPenality(IPlayerEntity player)
        {
            if (this._worldConfiguration.Death.DeathPenalityEnabled)
            {
                decimal expLossPercent = this._gameResources.Penalities.GetDecExpPenality(player.Object.Level);

                if (expLossPercent <= 0)
                    return;

                player.PlayerData.Experience -= player.PlayerData.Experience * (long)(expLossPercent / 100m);
                player.PlayerData.DeathLevel = player.Object.Level;

                if (player.PlayerData.Experience < 0)
                {
                    if (this._gameResources.Penalities.GetLevelDownPenality(player.Object.Level))
                    {
                        CharacterExpTableData previousLevelExp = this._gameResources.ExpTables.GetCharacterExp(player.Object.Level - 1);

                        player.Object.Level--;
                        player.PlayerData.Experience = previousLevelExp.Exp + player.PlayerData.Experience;
                    }
                    else
                    {
                        player.PlayerData.Experience = 0;
                    }
                }

                this._playerPacketFactory.SendPlayerExperience(player);
            }
        }
    }
}
