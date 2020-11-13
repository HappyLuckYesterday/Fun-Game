using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Map;

namespace Rhisis.Game.Abstractions.Entities
{
    /// <summary>
    /// Describes the monster entity.
    /// </summary>
    public interface IMonster : IMover
    {
        /// <summary>
        /// Gets a boolean value that indicates if the monster is aggresive.
        /// </summary>
        bool IsAggresive { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the monster is flying.
        /// </summary>
        bool IsFlying { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the monster can respawn. 
        /// </summary>
        bool CanRespawn { get; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the monster is returning to its initial position.
        /// </summary>
        bool IsReturningToBeginPosition { get; set; }

        /// <summary>
        /// Gets the monster initial position.
        /// </summary>
        Vector3 BeginPosition { get; }

        /// <summary>
        /// Gets the monster respawn region.
        /// </summary>
        IMapRespawnRegion RespawnRegion { get; }

        /// <summary>
        /// Gets the monster timers.
        /// </summary>
        IMonsterTimers Timers { get; }

        /// <summary>
        /// Gets the monster battle component.
        /// </summary>
        IBattle Battle { get; }
    }
}
