using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class MeleeSkillAttackArbiter : SkillAttackArbiterBase
    {
        public MeleeSkillAttackArbiter(IMover attacker, IMover defender, ISkill skill) 
            : base(attacker, defender, skill)
        {
        }

        public override AttackResult CalculateDamages()
        {
            var damages = (int)(GetAttackerSkillPower() * GetAttackMultiplier()) + Attacker.Attributes.Get(DefineAttributes.ATKPOWER);

            return AttackResult.Success(damages, AttackFlags.AF_MELEESKILL);
        }
    }
}
