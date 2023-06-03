using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;
using System.Diagnostics;

namespace Rhisis.Game.Battle.Projectiles;

[DebuggerDisplay("Magic Skill Projectile: {Skill}")]
public class MagicSkillProjectile : Projectile
{
    public override AttackFlags Type => AttackFlags.AF_MAGICSKILL;

    public Skill Skill { get; }

    public MagicSkillProjectile(Mover owner, Mover target, Skill skill, Action onArrived)
        : base(owner, target, onArrived)
    {
        Skill = skill;
    }
}
