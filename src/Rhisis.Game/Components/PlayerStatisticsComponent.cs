using Microsoft.Extensions.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using System;

namespace Rhisis.Game.Abstractions.Components
{
    public class PlayerStatisticsComponent : StatisticsComponent, IPlayerStatistics
    {
        private readonly IPlayer _player;
        private readonly Lazy<IStatisticsSystem> _statisticsSystem;

        public ushort AvailablePoints { get; set; }

        public PlayerStatisticsComponent(IPlayer player)
        {
            _player = player;
            _statisticsSystem = new Lazy<IStatisticsSystem>(() =>_player.Systems.GetService<IStatisticsSystem>());
        }

        public void UpdateStatistics(int strength, int stamina, int dexterity, int intelligence)
        {
            _statisticsSystem.Value.UpdateStatistics(_player, strength, stamina, dexterity, intelligence);
        }

        public void Restat()
        {
            _statisticsSystem.Value.Restat(_player);
        }
    }
}
