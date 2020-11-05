using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Map;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IMonster : IMover
    {
        bool IsAggresive { get; }

        bool IsFlying { get; }

        bool CanRespawn { get; }

        bool IsReturningToBeginPosition { get; set; }

        Vector3 BeginPosition { get; }

        IMapRespawnRegion RespawnRegion { get; }

        IMonsterTimers Timers { get; }

        IBattle Battle { get; }
    }
}
