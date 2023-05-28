using Rhisis.Game.Common;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Battle.AttackArbiters;

public class MeleeSkillAttackArbiter : SkillAttackArbiterBase
{
    public MeleeSkillAttackArbiter(Mover attacker, Mover defender, Skill skill)
        : base(attacker, defender, skill)
    {
    }

    public override AttackResult CalculateDamages()
    {
        var damages = (int)(GetAttackerSkillPower() * GetAttackMultiplier()) + Attacker.Attributes.Get(DefineAttributes.DST_ATKPOWER);

        return AttackResult.Success(damages, AttackFlags.AF_MELEESKILL);
    }
}
