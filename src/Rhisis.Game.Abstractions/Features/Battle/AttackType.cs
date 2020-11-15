using System.Collections.Generic;
using System.Text;

namespace Rhisis.Game.Abstractions.Features.Battle
{
    public enum AttackType
    {
        // TODO could there be a single MeleeAttack type with a sub type somehow?
        MeleeAttack1 = 0,
        MeleeAttack2 = 1,
        MeleeAttack3 = 2,
        MeleeAttack4 = 3,
        RangeBowAttack = 4,
        RangeWandAttack = 5,
        SkillMeleeAttack = 6,
        SkillMagicAttack = 7
    }
}
