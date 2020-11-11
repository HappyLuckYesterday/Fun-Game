using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features.AttackArbiters.Reducers
{
    public class MeleeSkillAttackReducer : AttackReducerBase
    {
        private static readonly IEnumerable<int> SkillsIgnoringDefense = new int[]
        {
            DefineSkill.SI_BIL_PST_ASALRAALAIKUM,
            DefineSkill.SI_JST_YOYO_HITOFPENYA
        };
        private readonly ISkill _skill;

        public MeleeSkillAttackReducer(IMover attacker, IMover defender, ISkill skill) 
            : base(attacker, defender)
        {
            _skill = skill;
        }

        public override AttackResult ReduceDamages(AttackResult attackResult)
        {
            int damages = attackResult.Damages;

            if (damages > 0 && !SkillsIgnoringDefense.Any(x => x == _skill.Id))
            {
                damages -= Math.Max(GetDefenderDefense(attackResult.Flags), 0);
            }

            return AttackResult.Success(Math.Max(0, damages), AttackFlags.AF_MELEESKILL);
        }
    }
}
