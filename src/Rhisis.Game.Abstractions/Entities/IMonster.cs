using Rhisis.Game.Abstractions.Components;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IMonster : IMover
    {
        bool IsAggresive { get; }

        IStatistics Statistics { get; }
    }
}
