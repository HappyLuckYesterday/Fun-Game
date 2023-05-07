using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Battle.AttackArbiters;

public class AttackArbiterBase
{
    /// <summary>
    /// Gets the attack mover.
    /// </summary>
    public Mover Attacker { get; }

    /// <summary>
    /// Gets the defender mover.
    /// </summary>
    public Mover Defender { get; }

    protected AttackArbiterBase(Mover attacker, Mover defender)
    {
        Attacker = attacker;
        Defender = defender;
    }

    /// <summary>
    /// Calculates the damages to inflict to the given defender.
    /// </summary>
    public virtual AttackResult CalculateDamages() => AttackResult.Miss();

    /// <summary>
    /// Gets the escape rating of an entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public int GetEspaceRating(Mover entity)
    {
        if (entity is Player player)
        {
            int playerDexterity = player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DST_DEX);

            return (int)(playerDexterity * 0.5f); // TODO: add DST_PARRY
        }
        else if (entity is Monster monster)
        {
            return monster.Properties.EscapeRating;
        }

        return default;
    }

    /// <summary>
    /// Gets the weapon attack damages based on player's statistics.
    /// </summary>
    /// <param name="player">Player</param>
    /// <param name="weaponType">Weapon type</param>
    /// <returns></returns>
    public int GetWeaponAttackDamages(Player player, WeaponType weaponType)
    {
        float attribute = 0f;
        float levelFactor = 0f;
        float jobFactor = 1f;

        switch (weaponType)
        {
            case WeaponType.MELEE_SWD:
                attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR) - 12;
                levelFactor = player.Level * 1.1f;
                jobFactor = player.Job.MeleeSword;
                break;
            case WeaponType.MELEE_AXE:
                attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR) - 12;
                levelFactor = player.Level * 1.2f;
                jobFactor = player.Job.MeleeAxe;
                break;
            case WeaponType.MELEE_STAFF:
                attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR) - 10;
                levelFactor = player.Level * 1.1f;
                jobFactor = player.Job.MeleeStaff;
                break;
            case WeaponType.MELEE_STICK:
                attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR) - 10;
                levelFactor = player.Level * 1.3f;
                jobFactor = player.Job.MeleeStick;
                break;
            case WeaponType.MELEE_KNUCKLE:
                attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR) - 10;
                levelFactor = player.Level * 1.2f;
                jobFactor = player.Job.MeleeKnuckle;
                break;
            case WeaponType.MAGIC_WAND:
                attribute = player.Statistics.Intelligence + player.Attributes.Get(DefineAttributes.DST_INT) - 10;
                levelFactor = player.Level * 1.2f;
                jobFactor = player.Job.MagicWand;
                break;
            case WeaponType.MELEE_YOYO:
                attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR) - 10;
                levelFactor = player.Level * 1.1f;
                jobFactor = player.Job.MeleeYoyo;
                break;
            case WeaponType.RANGE_BOW:
                attribute = (player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DST_DEX) - 14) * 4f;
                levelFactor = player.Level * 1.3f;
                jobFactor = (player.Statistics.Strength + player.Attributes.Get(DefineAttributes.DST_STR)) * 0.2f * 0.7f;
                break;
        }

        return (int)(attribute * jobFactor + levelFactor);
    }

    /// <summary>
    /// Gets the weapon attack power.
    /// </summary>
    /// <param name="entity">Living entity using the weapon.</param>
    /// <param name="weapon">Weapon used by the living entity.</param>
    /// <returns><see cref="AttackResult"/> with AttackMin and AttackMax range.</returns>
    public Range<int> GetWeaponAttackPower(Mover entity, Item weapon)
    {
        float multiplier = GetWeaponItemMultiplier(weapon);
        int power = weapon?.Refine > 0 ? (int)Math.Pow(weapon?.Refine ?? default, 1.5f) : default;

        return new Range<int>(
            (int)((entity.Attributes.Get(DefineAttributes.DST_ABILITY_MIN) + weapon?.Properties.AbilityMin) * multiplier) + power,
            (int)((entity.Attributes.Get(DefineAttributes.DST_ABILITY_MAX) + weapon?.Properties.AbilityMax) * multiplier) + power
        );
    }

    /// <summary>
    /// Gets the weapon item multiplier.
    /// </summary>
    /// <param name="weapon">Current used weapon.</param>
    /// <returns></returns>
    public float GetWeaponItemMultiplier(Item weapon)
    {
        if (weapon is null)
        {
            return 1f;
        }

        // TODO: check if item has expired.
        float multiplier = 1.0f;
        int refine = weapon.Properties.WeaponKind == WeaponKindType.Ultimate ? Item.ItemConstants.WeaponArmonRefineMax : weapon.Refine;

        if (refine > 0)
        {
            // TODO: get item exp up
            int itemExpUp = 0 + 100;
            multiplier *= itemExpUp / 100;
        }

        return multiplier;
    }

    /// <summary>
    /// Gets the weapon extra damages based weapon type and entity attribute bonuses.
    /// </summary>
    /// <param name="entity">Current entity.</param>
    /// <param name="weapon">Current entity weapon.</param>
    /// <returns>Weapon extra damages</returns>
    public int GetWeaponExtraDamages(Mover entity, Item weapon)
    {
        if (weapon is null)
        {
            return default;
        }

        int extraDamages = weapon.Properties.WeaponType switch
        {
            WeaponType.MELEE_SWD => entity.Attributes.Get(DefineAttributes.DST_SWD_DMG) + entity.Attributes.Get(DefineAttributes.DST_TWOHANDMASTER_DMG),
            WeaponType.MELEE_AXE => entity.Attributes.Get(DefineAttributes.DST_AXE_DMG) + entity.Attributes.Get(DefineAttributes.DST_TWOHANDMASTER_DMG),
            WeaponType.KNUCKLE => entity.Attributes.Get(DefineAttributes.DST_KNUCKLE_DMG) + entity.Attributes.Get(DefineAttributes.DST_KNUCKLEMASTER_DMG),
            WeaponType.MELEE_YOYO => entity.Attributes.Get(DefineAttributes.DST_YOY_DMG) + entity.Attributes.Get(DefineAttributes.DST_YOYOMASTER_DMG),
            WeaponType.RANGE_BOW => entity.Attributes.Get(DefineAttributes.DST_BOW_DMG) + entity.Attributes.Get(DefineAttributes.DST_BOWMASTER_DMG),
            _ => default
        };

        if (entity is Player player)
        {
            // TODO: check if player has dual weapons
            // TODO: if yes add "ONEHANDMASTER_DMG" to extra damages
        }

        return extraDamages;
    }

    /// <summary>
    /// Gets attacker attack multiplier.
    /// </summary>
    /// <returns></returns>
    public float GetAttackMultiplier()
    {
        var multiplier = 1.0f + Attacker.Attributes.Get(DefineAttributes.DST_ATKPOWER_RATE) / 100f;

        if (Attacker is Player)
        {
            // TODO: check SM mode SM_ATTACK_UP or SM_ATTACK_UP1 => multiplier *= 1.2f;

            var attribute = Defender is Player ? DefineAttributes.DST_PVP_DMG : DefineAttributes.DST_MONSTER_DMG;
            int damages = Attacker.Attributes.Get(attribute);

            if (damages > 0)
            {
                multiplier += multiplier * damages / 100;
            }
        }

        return multiplier;
    }
}
