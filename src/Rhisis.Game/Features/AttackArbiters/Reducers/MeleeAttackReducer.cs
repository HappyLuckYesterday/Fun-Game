using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Features.AttackArbiters.Reducers
{
    public class MeleeAttackReducer : AttackReducerBase
    {
        public MeleeAttackReducer(IMover attacker, IMover defender) 
            : base(attacker, defender)
        {
        }

        public override AttackResult ReduceDamages(AttackResult attackResult)
        {
            int damages = attackResult.Damages;
            AttackFlags flags = attackResult.Flags;

            if (flags.HasFlag(AttackFlags.AF_MAGICSKILL))
            {
                damages -= Defender.Attributes.Get(DefineAttributes.RESIST_MAGIC);
            }

            damages -= GetDefenderDefense(flags);
            damages = Math.Max(0, damages);

            if (damages > 0)
            {
                float blockFactor = GetDefenderBlockFactor();

                if (blockFactor < 1f)
                {
                    attackResult.Flags |= AttackFlags.AF_BLOCKING;
                    attackResult.Damages = (int)(attackResult.Damages * blockFactor);
                }
            }
            else
            {
                flags &= ~AttackFlags.AF_CRITICAL;
                flags &= ~AttackFlags.AF_FLYING;
            }

            return AttackResult.Success(damages, flags);
        }
    }
}
