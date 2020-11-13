using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game
{
    public class MagicSkillProjectile : Projectile, IMagicSkillProjectile
    {
        public override AttackFlags Type => AttackFlags.AF_MAGICSKILL;

        public ISkill Skill { get; }

        public MagicSkillProjectile(IMover owner, IMover target, ISkill skill, Action onArrived)
            : base(owner, target, onArrived)
        {
            Skill = skill;
        }
    }
}