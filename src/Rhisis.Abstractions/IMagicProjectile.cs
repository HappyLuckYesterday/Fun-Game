﻿namespace Rhisis.Abstractions;

public interface IMagicProjectile : IProjectile
{
    /// <summary>
    /// Gets the projectile magic power.
    /// </summary>
    int MagicPower { get; }
}
