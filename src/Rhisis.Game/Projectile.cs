using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;
using System.Diagnostics;

namespace Rhisis.Game
{
    [DebuggerDisplay("{Owner}'s projectile {Type}")]
    public class Projectile : IProjectile
    {
        public IMover Owner { get; }

        public IMover Target { get; }

        public Action OnArrived { get; }

        public virtual AttackFlags Type => AttackFlags.AF_GENERIC;

        public Projectile(IMover owner, IMover target, Action onArrived)
        {
            Owner = owner;
            Target = target;
            OnArrived = onArrived;
        }
    }
}