using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class SkillAttackArbiterBase : AttackArbiterBase
    {
        protected ISkill Skill { get; }
     
        protected SkillAttackArbiterBase(IMover attacker, IMover defender, ISkill skill)
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
            int referStatistic1 = Attacker.Attributes.Get(Skill.Data.ReferStat1);
            int referStatistic2 = Attacker.Attributes.Get(Skill.Data.ReferStat2);

            if (Skill.Data.ReferTarget1 == SkillReferTargetType.Attack && referStatistic1 != 0)
            {
                referStatistic1 = (int)(Skill.Data.ReferValue1 / 10f * referStatistic1 + Skill.Level * (referStatistic1 / 50f));
            }

            if (Skill.Data.ReferTarget2 == SkillReferTargetType.Attack && referStatistic2 != 0)
            {
                referStatistic2 = (int)(Skill.Data.ReferValue2 / 10f * referStatistic2 + Skill.Level * (referStatistic2 / 50f));
            }

            var referStatistic = referStatistic1 + referStatistic2;
            Range<int> attack = Attacker is IPlayer && Defender is IPlayer
                ? new Range<int>(Skill.LevelData.AbilityMinPVP, Skill.LevelData.AbilityMaxPVP)
                : new Range<int>(Skill.LevelData.AbilityMin, Skill.LevelData.AbilityMax);

            IItem weaponItem = null;

            if (Attacker is IPlayer player)
            {
                weaponItem = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? player.Inventory.Hand;
            }

            Range<int> weaponAttackPower = GetWeaponAttackPower(Attacker, weaponItem);
            var weaponExtraDamages = GetWeaponExtraDamages(Attacker, weaponItem);

            attack = new Range<int>(attack.Minimum + weaponItem.Data.AttackSkillMin, attack.Maximum + weaponItem.Data.AttackSkillMax);

            float powerMin = (weaponAttackPower.Minimum + attack.Minimum * 5 + referStatistic - 20) * (16 + Skill.Level) / 13;
            float powerMax = (weaponAttackPower.Maximum + attack.Maximum * 5 + referStatistic - 20) * (16 + Skill.Level) / 13;

            // TODO: check CHR_DMG
            powerMin += weaponExtraDamages;
            powerMax += weaponExtraDamages;

            float attackMinMax = Math.Max(powerMax - powerMin + 1, 1);

            return (int)(powerMin + RandomHelper.FloatRandom(1f, attackMinMax));
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
}
