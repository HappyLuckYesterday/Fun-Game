using Rhisis.Game.Components;

namespace Rhisis.Game.Entities
{
    public interface IPlayer : IHuman
    {
        long Experience { get; set; }

        StatisticsComponent Statistics { get; }
    }
}
