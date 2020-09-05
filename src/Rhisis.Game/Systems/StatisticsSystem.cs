using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using System;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public sealed class StatisticsSystem : IStatisticsSystem
    {
        public const int DefaultStatisticValue = 1;

        public void UpdateStatistics(IPlayer player, int strength, int stamina, int dexterity, int intelligence)
        {
            ushort statsPoints = player.Statistics.AvailablePoints;
            int total = strength + stamina + dexterity + intelligence;

            if (statsPoints <= 0 || total > statsPoints)
            {
                throw new InvalidOperationException($"{player.Name} dosn't have enough statistic points.");
            }

            if (strength > statsPoints || stamina > statsPoints ||
                dexterity > statsPoints || intelligence > statsPoints || total <= 0 ||
                total > ushort.MaxValue)
            {
                throw new InvalidOperationException("Statistics point bad calculation. (Hack attempt)");
            }

            player.Statistics.Strength += strength;
            player.Statistics.Stamina += stamina;
            player.Statistics.Dexterity += dexterity;
            player.Statistics.Intelligence += intelligence;
            player.Statistics.AvailablePoints -= (ushort)total;

            // TODO: send update packet
        }

        public void Restat(IPlayer player)
        {
            const int DefaultAttributePoints = 15;

            player.Statistics.Strength = DefaultAttributePoints;
            player.Statistics.Stamina = DefaultAttributePoints;
            player.Statistics.Dexterity = DefaultAttributePoints;
            player.Statistics.Intelligence = DefaultAttributePoints;
            player.Statistics.AvailablePoints = (ushort)((player.Level - 1) * 2);

            // TODO: send update packet
        }
    }
}
