using Rhisis.Game.Abstractions.Components;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IPlayer : IHuman
    {
        long Experience { get; set; }

        int Gold { get; set; }

        StatisticsComponent Statistics { get; }
    }
}
