using Rhisis.Game.Common;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Battle.AttackArbiters.Reducers;

public class MagicSkillAttackReducer : AttackReducerBase
{
    private readonly Skill _skill;

    public MagicSkillAttackReducer(Mover attacker, Mover defender, Skill skill)
        : base(attacker, defender)
    {
        _skill = skill;
    }

    public override AttackResult ReduceDamages(AttackResult attackResult)
    {
        int damages = attackResult.Damages;
        int defenderDefense = GetDefenderDefense(attackResult.Flags);

        damages -= damages * (int)(Defender.Attributes.Get(DefineAttributes.DST_RESIST_MAGIC_RATE) / 100f);

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
    private float GetMagicSkillResistance(Mover entity, Skill skill)
    {
        var skillResistance = skill.Properties.Element switch
        {
            ElementType.Fire => entity.Properties.FireResitance + entity.Attributes.Get(DefineAttributes.DST_RESIST_FIRE),
            ElementType.Water => entity.Properties.WaterResistance + entity.Attributes.Get(DefineAttributes.DST_RESIST_WATER),
            ElementType.Electricity => entity.Properties.ElectricityResistance + entity.Attributes.Get(DefineAttributes.DST_RESIST_ELECTRICITY),
            ElementType.Wind => entity.Properties.WindResistance + entity.Attributes.Get(DefineAttributes.DST_RESIST_WIND),
            ElementType.Earth => entity.Properties.EarthResistance + entity.Attributes.Get(DefineAttributes.DST_RESIST_EARTH),
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
    private float GetMagicSkillResitanceRate(Mover entity, Skill skill) => GetMagicSkillResistance(entity, skill) / 100f;

    /// <summary>
    /// Gets the magic skill factor based on the equiped weapon element type and skill element type.
    /// </summary>
    /// <param name="entity">Living entity.</param>
    /// <param name="skill">Skill.</param>
    /// <returns>Magic skill factor.</returns>
    private float GetMagicSkillFactor(Mover entity, Skill skill)
    {
        const float DefaultMagicSkillFactor = 1.0f;

        if (entity is Player player)
        {
            Item weapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);
            ElementType weaponElementType = weapon.Id != -1 ? weapon.Properties.Element : ElementType.None;
            ElementType skillElementType = skill.Properties.Element.GetValueOrDefault(ElementType.None);

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
