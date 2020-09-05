using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IStatisticsSystem
    {
        void UpdateStatistics(IPlayer player, int strength, int stamina, int dexterity, int intelligence);

        void Restat(IPlayer player);
    }
}
