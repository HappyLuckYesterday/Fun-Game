using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class MagicSkillAttackArbiter : SkillAttackArbiterBase
    {
        public MagicSkillAttackArbiter(IMover attacker, IMover defender, ISkill skill) 
            : base(attacker, defender, skill)
        {
        }

        public override AttackResult CalculateDamages()
        {
            int damages = GetAttackerSkillPower();
            DefineAttributes? skillMastryAttribute = Skill.Data.SpellType switch
            {
                SpellType.Fire => DefineAttributes.MASTRY_FIRE,
                SpellType.Water => DefineAttributes.MASTRY_WATER,
                SpellType.Electricity => DefineAttributes.MASTRY_ELECTRICITY,
                SpellType.Wind => DefineAttributes.MASTRY_WIND,
                SpellType.Earth => DefineAttributes.MASTRY_EARTH,
                _ => default
            };

            if (skillMastryAttribute.HasValue)
            {
                var ratio = Math.Max(0, Attacker.Attributes.Get(skillMastryAttribute.Value) / 100f);

                damages = (int)(damages + damages * ratio);
            }

            damages *= (int)GetAttackMultiplier();
            damages += Attacker.Attributes.Get(DefineAttributes.ATKPOWER);

            return AttackResult.Success(damages, AttackFlags.AF_MAGICSKILL);
        }
    }
}
