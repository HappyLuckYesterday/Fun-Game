using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Abstractions
{
    public interface IProjectile
    {
        /// <summary>
        /// Gets the projectile owner.
        /// </summary>
        IMover Owner { get; }

        /// <summary>
        /// Gets the projectile target.
        /// </summary>
        IMover Target { get; }

        /// <summary>
        /// Gets the projectile action to execute when it hits the target.
        /// </summary>
        Action OnArrived { get; }

        /// <summary>
        /// Gets the projectile type.
        /// </summary>
        AttackFlags Type { get; }
    }
}
