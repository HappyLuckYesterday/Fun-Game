using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Battle.AttackArbiters;

public class MagicSkillAttackArbiter : SkillAttackArbiterBase
{
    public MagicSkillAttackArbiter(Mover attacker, Mover defender, Skill skill)
        : base(attacker, defender, skill)
    {
    }

    public override AttackResult CalculateDamages()
    {
        int damages = GetAttackerSkillPower();
        DefineAttributes? skillMastryAttribute = Skill.Properties.SpellType switch
        {
            SpellType.Fire => DefineAttributes.DST_MASTRY_FIRE,
            SpellType.Water => DefineAttributes.DST_MASTRY_WATER,
            SpellType.Electricity => DefineAttributes.DST_MASTRY_ELECTRICITY,
            SpellType.Wind => DefineAttributes.DST_MASTRY_WIND,
            SpellType.Earth => DefineAttributes.DST_MASTRY_EARTH,
            _ => default
        };

        if (skillMastryAttribute.HasValue)
        {
            var ratio = Math.Max(0, Attacker.Attributes.Get(skillMastryAttribute.Value) / 100f);

            damages = (int)(damages + damages * ratio);
        }

        damages *= (int)GetAttackMultiplier();
        damages += Attacker.Attributes.Get(DefineAttributes.DST_ATKPOWER);

        return AttackResult.Success(damages, AttackFlags.AF_MAGICSKILL);
    }
}
