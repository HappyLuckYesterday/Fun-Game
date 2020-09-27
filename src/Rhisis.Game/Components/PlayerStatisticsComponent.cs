using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network.Snapshots;
using System;

namespace Rhisis.Game.Abstractions.Components
{
    public class PlayerStatisticsComponent : StatisticsComponent, IPlayerStatistics
    {
        private readonly IPlayer _player;

        public ushort AvailablePoints { get; set; }

        public PlayerStatisticsComponent(IPlayer player)
        {
            _player = player;
        }

        public void UpdateStatistics(int strength, int stamina, int dexterity, int intelligence)
        {
            ushort statsPoints = AvailablePoints;
            int total = strength + stamina + dexterity + intelligence;

            if (statsPoints <= 0 || total > statsPoints)
            {
                throw new InvalidOperationException($"{_player.Name} dosn't have enough statistic points.");
            }

            if (strength > statsPoints || stamina > statsPoints ||
                dexterity > statsPoints || intelligence > statsPoints || total <= 0 ||
                total > ushort.MaxValue)
            {
                throw new InvalidOperationException("Statistics point bad calculation. (Hack attempt)");
            }

            Strength += strength;
            Stamina += stamina;
            Dexterity += dexterity;
            Intelligence += intelligence;
            AvailablePoints -= (ushort)total;

            SendState();
        }

        public void Restat()
        {
            const int DefaultAttributePoints = 15;

            Strength = DefaultAttributePoints;
            Stamina = DefaultAttributePoints;
            Dexterity = DefaultAttributePoints;
            Intelligence = DefaultAttributePoints;
            AvailablePoints = (ushort)((_player.Level - 1) * 2);

            SendState();
        }

        private void SendState()
        {
            using var setStateSnapshot = new SetStatisticsStateSnapshot(_player);

            _player.Send(setStateSnapshot);
        }
    }
}
