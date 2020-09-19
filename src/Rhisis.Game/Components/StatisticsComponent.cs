namespace Rhisis.Game.Abstractions.Components
{
    public class StatisticsComponent : IStatistics
    {
        public int Strength { get; set; }

        public int Stamina { get; set; }

        public int Dexterity { get; set; }

        public int Intelligence { get; set; }
    }
}
