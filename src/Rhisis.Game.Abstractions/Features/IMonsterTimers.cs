namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the monster timers.
    /// </summary>
    public interface IMonsterTimers
    {
        /// <summary>
        /// Gets or sets the monster next move time.
        /// </summary>
        long NextMoveTime { get; set; }

        /// <summary>
        /// Gets or sets the monster next attack time.
        /// </summary>
        long NextAttackTime { get; set; }

        /// <summary>
        /// Gets or sets the monster despawn time.
        /// </summary>
        long DespawnTime { get; set; }

        /// <summary>
        /// Gets or sets the monster respawn time.
        /// </summary>
        long RespawnTime { get; set; }
    }
}
