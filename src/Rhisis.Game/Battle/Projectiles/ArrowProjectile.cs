using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;
using System.Diagnostics;

namespace Rhisis.Game.Battle.Projectiles;

[DebuggerDisplay("Arrow Projectile: {Power}")]
public class ArrowProjectile : Projectile
{
    public override AttackFlags Type => AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE;

    public int Power { get; }

    public ArrowProjectile(Mover owner, Mover target, int power, Action onArrived)
        : base(owner, target, onArrived)
    {
        Power = power;
    }
}
