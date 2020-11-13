using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Features.AttackArbiters.Reducers
{
    public class MagicSkillAttackReducer : AttackReducerBase
    {
        private readonly ISkill _skill;

        public MagicSkillAttackReducer(IMover attacker, IMover defender, ISkill skill) 
            : base(attacker, defender)
        {
            _skill = skill;
        }

        public override AttackResult ReduceDamages(AttackResult attackResult)
        {
            int damages = attackResult.Damages;
            int defenderDefense = GetDefenderDefense(attackResult.Flags);

            damages -= damages * (int)(Defender.Attributes.Get(DefineAttributes.RESIST_MAGIC_RATE) / 100f);

            var damageDefenseDelta = (damages - defenderDefense) * (1.0f - GetMagicSkillResitanceRate(Defender, _skill));

            damages = (int)(damageDefenseDelta * GetMagicSkillFactor(Attacker, _skill));

            return AttackResult.Success(damages, AttackFlags.AF_MAGICSKILL);
        }

        /// <summary>
        /// Gets the magic skill resistance for a given living entity.
        /// </summary>
        /// <param name="entity">Living entity.</param>
        /// <param name="skill">Skill.</param>
        /// <returns>Magic skill resistance.</returns>
        private float GetMagicSkillResistance(IMover entity, ISkill skill)
        {
            var skillResistance = skill.Data.Element switch
            {
                ElementType.Fire => entity.Data.FireResitance + entity.Attributes.Get(DefineAttributes.RESIST_FIRE),
                ElementType.Water => entity.Data.WaterResistance + entity.Attributes.Get(DefineAttributes.RESIST_WATER),
                ElementType.Electricity => entity.Data.ElectricityResistance + entity.Attributes.Get(DefineAttributes.RESIST_ELECTRICITY),
                ElementType.Wind => entity.Data.WindResistance + entity.Attributes.Get(DefineAttributes.RESIST_WIND),
                ElementType.Earth => entity.Data.EarthResistance + entity.Attributes.Get(DefineAttributes.RESIST_EARTH),
                _ => default
            };

            return skillResistance;
        }

        /// <summary>
        /// Gets the magic skill resistance rate for a given living entity.
        /// </summary>
        /// <param name="entity">Living entity.</param>
        /// <param name="skill">Skill.</param>
        /// <returns>Magic skill resistance rate.</returns>
        private float GetMagicSkillResitanceRate(IMover entity, ISkill skill) => GetMagicSkillResistance(entity, skill) / 100f;

        /// <summary>
        /// Gets the magic skill factor based on the equiped weapon element type and skill element type.
        /// </summary>
        /// <param name="entity">Living entity.</param>
        /// <param name="skill">Skill.</param>
        /// <returns>Magic skill factor.</returns>
        private float GetMagicSkillFactor(IMover entity, ISkill skill)
        {
            const float DefaultMagicSkillFactor = 1.0f;

            if (entity is IPlayer player)
            {
                IItem weapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? player.Inventory.Hand;
                ElementType weaponElementType = weapon.Id != -1 ? weapon.Data.Element : ElementType.None;
                ElementType skillElementType = skill.Data.Element.GetValueOrDefault(ElementType.None);

                if (skillElementType == weaponElementType)
                {
                    return 1.1f;
                }
                else if (
                    skillElementType == ElementType.Fire && weaponElementType == ElementType.Water ||
                    skillElementType == ElementType.Water && weaponElementType == ElementType.Electricity ||
                    skillElementType == ElementType.Electricity && weaponElementType == ElementType.Earth ||
                    skillElementType == ElementType.Wind && weaponElementType == ElementType.Fire
                    )
                {
                    return 0.9f;
                }
            }

            return DefaultMagicSkillFactor;
        }
    }
}
