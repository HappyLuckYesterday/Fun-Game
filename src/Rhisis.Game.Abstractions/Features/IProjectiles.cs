using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage projectiles.
    /// </summary>
    public interface IProjectiles : IReadOnlyDictionary<int, IProjectile>
    {
        /// <summary>
        /// Adds a new projectile.
        /// </summary>
        /// <param name="projectile">Projectile to add.</param>
        /// <returns>The projectile id.</returns>
        int Add(IProjectile projectile);

        /// <summary>
        /// Removes a projectile by its id.
        /// </summary>
        /// <param name="projectileId">Projectile id.</param>
        void Remove(int projectileId);

        /// <summary>
        /// Resets the mover's projectiles.
        /// </summary>
        void Reset();
    }
}
