using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Battle;

public class Projectile
{
    /// <summary>
    /// Gets the projectile owner.
    /// </summary>
    public Mover Owner { get; }

    /// <summary>
    /// Gets the projectile target.
    /// </summary>
    public Mover Target { get; }

    /// <summary>
    /// Gets the action to execute when the projectile has arrived to its target.
    /// </summary>
    public Action OnArrived { get; }

    /// <summary>
    /// Gets the projectile attack type.
    /// </summary>
    public virtual AttackFlags Type => AttackFlags.AF_GENERIC;

    /// <summary>
    /// Creates a new <see cref="Projectile"/> instance.
    /// </summary>
    /// <param name="owner">Projectile owner.</param>
    /// <param name="target">Projectile target.</param>
    /// <param name="onArrived">Projectile action to execute when arrived to its target.</param>
    public Projectile(Mover owner, Mover target, Action onArrived)
    {
        Owner = owner;
        Target = target;
        OnArrived = onArrived;
    }
}
