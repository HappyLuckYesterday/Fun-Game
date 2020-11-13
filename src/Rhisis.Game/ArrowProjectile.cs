using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game
{
    public class ArrowProjectile : Projectile, IArrowProjectile
    {
        public override AttackFlags Type => AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE;

        public int Power { get; }

        public ArrowProjectile(IMover owner, IMover target, int power, Action onArrived) 
            : base(owner, target, onArrived)
        {
            Power = power;
        }
    }
}