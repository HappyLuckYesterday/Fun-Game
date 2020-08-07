using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.PlayerData;
using System;

namespace Rhisis.World.Systems.Statistics
{
    [Injectable]
    public sealed class StatisticsSystem : IStatisticsSystem
    {
        public const int DefaultStatisticValue = 1;

        private readonly ILogger<StatisticsSystem> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;
        private readonly IPlayerPacketFactory _playerPacketFactory;

        /// <summary>
        /// Creates a new <see cref="StatisticsSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        /// <param name="playerPacketFactory">Player packet factory.</param>
        public StatisticsSystem(ILogger<StatisticsSystem> logger, IPlayerDataSystem playerDataSystem, IPlayerPacketFactory playerPacketFactory)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
            _playerPacketFactory = playerPacketFactory;
        }

        public void UpdateStatistics(IPlayerEntity player, ushort strength, ushort stamina, ushort dexterity, ushort intelligence)
        {
            var total = strength + stamina + dexterity + intelligence;

            var statsPoints = player.Statistics.AvailablePoints;
            if (statsPoints <= 0 || total > statsPoints)
            {
                _logger.LogError("No statspoints available, but trying to upgrade {0}.", player.Object.Name);
                return;
            }

            if (strength > statsPoints || stamina > statsPoints ||
                dexterity > statsPoints || intelligence > statsPoints || total <= 0 ||
                total > ushort.MaxValue)
            {
                _logger.LogError("Invalid upgrade request due to bad total calculation (trying to dupe) {0}.",
                    player.Object.Name);
                return;
            }

            player.Statistics.Strength += strength;
            player.Statistics.Stamina += stamina;
            player.Statistics.Dexterity += dexterity;
            player.Statistics.Intelligence += intelligence;
            player.Statistics.AvailablePoints -= (ushort)total;

            _playerPacketFactory.SendPlayerUpdateState(player);
            _playerDataSystem.CalculateDefense(player);
        }

        public void Restat(IPlayerEntity player)
        {
            const int DefaultAttributePoints = 15;

            player.Statistics.Strength = DefaultAttributePoints;
            player.Statistics.Stamina = DefaultAttributePoints;
            player.Statistics.Dexterity = DefaultAttributePoints;
            player.Statistics.Intelligence = DefaultAttributePoints;
            player.Statistics.AvailablePoints = (ushort)((player.Object.Level - 1) * 2);

            _playerPacketFactory.SendPlayerUpdateState(player);
            _playerDataSystem.CalculateDefense(player);
        }

        #region Strength

        public int GetBaseStrength(ILivingEntity entity)
        {
            return entity switch
            {
                IPlayerEntity player => player.Statistics.Strength,
                IMonsterEntity monster => monster.Data.Strength,
                _ => 0
            };
        }

        public int GetAttributedStrength(ILivingEntity entity) => entity.Attributes[DefineAttributes.STR];

        public int GetTotalStrength(ILivingEntity entity)
        {
            return Math.Max(DefaultStatisticValue, GetBaseStrength(entity) + GetBaseStrength(entity));
        }

        #endregion

        #region Stamina

        public int GetBaseStamina(ILivingEntity entity)
        {
            return entity switch
            {
                IPlayerEntity player => player.Statistics.Stamina,
                IMonsterEntity monster => monster.Data.Stamina,
                _ => 0
            };
        }

        public int GetAttributedStamina(ILivingEntity entity) => entity.Attributes[DefineAttributes.STA];

        public int GetTotalStamina(ILivingEntity entity)
        {
            return Math.Max(DefaultStatisticValue, GetBaseStamina(entity) + GetAttributedStamina(entity));
        }

        #endregion

        #region Dexterity

        public int GetBaseDexterity(ILivingEntity entity)
        {
            return entity switch
            {
                IPlayerEntity player => player.Statistics.Dexterity,
                IMonsterEntity monster => monster.Data.Dexterity,
                _ => 0
            };
        }

        public int GetAttributedDexterity(ILivingEntity entity) => entity.Attributes[DefineAttributes.DEX];

        public int GetTotalDexterity(ILivingEntity entity)
        {
            return Math.Max(DefaultStatisticValue, GetBaseDexterity(entity) + GetAttributedDexterity(entity));
        }

        #endregion

        #region Intelligence

        public int GetBaseIntelligence(ILivingEntity entity)
        {
            return entity switch
            {
                IPlayerEntity player => player.Statistics.Intelligence,
                IMonsterEntity monster => monster.Data.Intelligence,
                _ => 0
            };
        }

        public int GetAttributedIntelligence(ILivingEntity entity) => entity.Attributes[DefineAttributes.INT];

        public int GetTotalIntelligence(ILivingEntity entity)
        {
            return Math.Max(DefaultStatisticValue, GetBaseIntelligence(entity) + GetAttributedIntelligence(entity));
        }

        #endregion
    }
}
