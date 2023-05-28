using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Battle.AttackArbiters;

public class SkillAttackArbiterBase : AttackArbiterBase
{
    protected Skill Skill { get; }

    protected SkillAttackArbiterBase(Mover attacker, Mover defender, Skill skill)
        : base(attacker, defender)
    {
        Skill = skill;
    }

    /// <summary>
    /// Gets the attacker skill power.
    /// </summary>
    /// <returns>Skill power.</returns>
    protected int GetAttackerSkillPower()
    {
        int referStatistic1 = Attacker.Attributes.Get(Skill.Properties.ReferStat1);
        int referStatistic2 = Attacker.Attributes.Get(Skill.Properties.ReferStat2);

        if (Skill.Properties.ReferTarget1 == SkillReferTargetType.Attack && referStatistic1 != 0)
        {
            referStatistic1 = (int)(Skill.Properties.ReferValue1 / 10f * referStatistic1 + Skill.Level * (referStatistic1 / 50f));
        }

        if (Skill.Properties.ReferTarget2 == SkillReferTargetType.Attack && referStatistic2 != 0)
        {
            referStatistic2 = (int)(Skill.Properties.ReferValue2 / 10f * referStatistic2 + Skill.Level * (referStatistic2 / 50f));
        }

        var referStatistic = referStatistic1 + referStatistic2;
        Range<int> attack = Attacker is Player && Defender is Player
            ? new Range<int>(Skill.LevelProperties.AbilityMinPVP, Skill.LevelProperties.AbilityMaxPVP)
            : new Range<int>(Skill.LevelProperties.AbilityMin, Skill.LevelProperties.AbilityMax);

        Item weaponItem = null;

        if (Attacker is Player player)
        {
            weaponItem = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);
        }

        Range<int> weaponAttackPower = GetWeaponAttackPower(Attacker, weaponItem);
        var weaponExtraDamages = GetWeaponExtraDamages(Attacker, weaponItem);

        attack = new Range<int>(attack.Minimum + weaponItem.Properties.AttackSkillMin, attack.Maximum + weaponItem.Properties.AttackSkillMax);

        float powerMin = (weaponAttackPower.Minimum + attack.Minimum * 5 + referStatistic - 20) * (16 + Skill.Level) / 13;
        float powerMax = (weaponAttackPower.Maximum + attack.Maximum * 5 + referStatistic - 20) * (16 + Skill.Level) / 13;

        // TODO: check CHR_DMG
        powerMin += weaponExtraDamages;
        powerMax += weaponExtraDamages;

        float attackMinMax = Math.Max(powerMax - powerMin + 1, 1);

        return (int)(powerMin + FFRandom.FloatRandom(1f, attackMinMax));
    }

    //protected override float GetDefenseMultiplier(float defenseFactor = 1)
    //{
    //    if (Skill.Id == DefineSkill.SI_BLD_DOUBLE_ARMORPENETRATE)
    //    {
    //        defenseFactor *= 0.5f;
    //    }

    //    return base.GetDefenseMultiplier(defenseFactor);
    //}
}
