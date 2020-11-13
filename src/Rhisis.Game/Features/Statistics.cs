using Rhisis.Game.Abstractions.Features;

namespace Rhisis.Game.Features
{
    public class Statistics : IStatistics
    {
        public int Strength { get; set; }

        public int Stamina { get; set; }

        public int Dexterity { get; set; }

        public int Intelligence { get; set; }
    }
}
