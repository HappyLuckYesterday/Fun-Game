using Microsoft.Extensions.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using System;

namespace Rhisis.Game.Abstractions.Components
{
    public class StatisticsComponent
    {
        private readonly IPlayer _player;
        private readonly Lazy<IStatisticsSystem> _statisticsSystem;

        /// <summary>
        /// Gets or sets the available statistics points.
        /// </summary>
        public ushort AvailablePoints { get; set; }

        /// <summary>
        /// Gets or sets the original strength points.
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// Gets or sets the original stamina points.
        /// </summary>
        public int Stamina { get; set; }

        /// <summary>
        /// Gets or sets the original dexterity points.
        /// </summary>
        public int Dexterity { get; set; }

        /// <summary>
        /// Gets or sets the orginal intelligence points.
        /// </summary>
        public int Intelligence { get; set; }

        public StatisticsComponent(IPlayer player)
        {
            _player = player;
            _statisticsSystem = new Lazy<IStatisticsSystem>(_player.Systems.GetService<IStatisticsSystem>());
        }

        public void SetStatistics(int strength, int stamina, int dexterity, int intelligence)
        {
            _statisticsSystem.Value.UpdateStatistics(_player, strength, stamina, dexterity, intelligence);
        }
    }
}
