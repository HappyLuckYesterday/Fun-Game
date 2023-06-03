using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;
using System.Diagnostics;

namespace Rhisis.Game.Battle.Projectiles;

[DebuggerDisplay("Magic Projectile: {MagicPower}")]
public class MagicProjectile : Projectile
{
    public override AttackFlags Type => AttackFlags.AF_MAGIC;

    public int MagicPower { get; }

    public MagicProjectile(Mover owner, Mover target, int magicPower, Action onArrived)
        : base(owner, target, onArrived)
    {
        MagicPower = magicPower;
    }
}
