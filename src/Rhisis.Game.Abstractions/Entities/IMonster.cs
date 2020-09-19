using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Map;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IMonster : IMover
    {
        bool IsAggresive { get; }

        bool IsFlying { get; }

        IStatistics Statistics { get; }

        bool CanRespawn { get; }

        IMapRespawnRegion RespawnRegion { get; }

        IMonsterTimers Timers { get; }
    }
}
