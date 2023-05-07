using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Battle.AttackArbiters;

public class MeleeAttackArbiter : AttackArbiterBase
{
    public const int MinimalHitRate = 20;
    public const int MaximalHitRate = 96;
    private readonly AttackFlags _attackFlags;
    private readonly int _attackPower;

    /// <summary>
    /// Creates a new <see cref="MeleeAttackArbiter"/> instance.
    /// </summary>
    /// <param name="attacker">Attacker entity</param>
    /// <param name="defender">Defender entity</param>
    /// <param name="attackFlags">Default attack flags.</param>
    /// <param name="attackPower">Attack power.</param>
    public MeleeAttackArbiter(Mover attacker, Mover defender, AttackFlags attackFlags = AttackFlags.AF_GENERIC, int attackPower = 0)
        : base(attacker, defender)
    {
        _attackFlags = attackFlags;
        _attackPower = attackPower;
    }

    /// <summary>
    /// Calculates the melee attack damages.
    /// </summary>
    /// <returns></returns>
    public override AttackResult CalculateDamages()
    {
        AttackFlags flags = GetAttackFlags();

        if (flags.HasFlag(AttackFlags.AF_MISS))
        {
            return AttackResult.Miss();
        }

        Range<int> attackRange = null;

        if (Attacker is Player player)
        {
            Item weapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);
            // TODO: get dual weapon for blades
            int weaponAttack = GetWeaponAttackDamages(player, weapon.Properties.WeaponType);

            attackRange = new Range<int>(weapon.Properties.AbilityMin * 2 + weaponAttack, weapon.Properties.AbilityMax * 2 + weaponAttack);
        }
        else if (Attacker is Monster monster)
        {
            attackRange = new Range<int>(monster.Properties.AttackMin, monster.Properties.AttackMax);
        }

        if (attackRange == null)
        {
            return AttackResult.Miss();
        }

        if (IsCriticalAttack(Attacker, flags))
        {
            flags |= AttackFlags.AF_CRITICAL;
            attackRange = CalculateCriticalDamages(attackRange);

            if (IsKnockback(flags))
            {
                flags |= AttackFlags.AF_FLYING;
            }
        }

        int damages = FFRandom.Random(attackRange.Minimum, attackRange.Maximum);

        if (flags.HasFlag(AttackFlags.AF_RANGE))
        {
            damages = (int)(damages * GetChargeAttackMultiplier());
        }

        return AttackResult.Success(damages, flags);
    }

    /// <summary>
    /// Gets the <see cref="AttackFlags"/> of this melee attack.
    /// </summary>
    /// <returns></returns>
    private AttackFlags GetAttackFlags()
    {
        int hitRate;
        var hitRating = GetHitRating(Attacker);
        var escapeRating = GetEspaceRating(Defender);

        if (Attacker is Monster && Defender is Player)
        {
            // Monster VS Player
            hitRate = (int)(hitRating * 1.5f / (hitRating + escapeRating) * 2.0f *
                      (Attacker.Level * 0.5f / (Attacker.Level + Defender.Level * 0.3f)) * 100.0f);
        }
        else
        {
            // Player VS Player or Player VS Monster
            hitRate = (int)(hitRating * 1.6f / (hitRating + escapeRating) * 1.5f *
                      (Attacker.Level * 1.2f / (Attacker.Level + Defender.Level)) * 100.0f);
        }

        hitRate = Math.Clamp(hitRate, MinimalHitRate, MaximalHitRate);

        return FFRandom.Random(0, 100) < hitRate ? _attackFlags : AttackFlags.AF_MISS;
    }

    /// <summary>
    /// Gets the hit rating of an entity.
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns></returns>
    private int GetHitRating(Mover entity)
    {
        if (entity is Player player)
        {
            return player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DST_DEX);
        }
        else if (entity is Monster monster)
        {
            return monster.Properties.HitRating;
        }

        return 0;
    }

    /// <summary>
    /// Check if the attacker's melee attack is a critical hit.
    /// </summary>
    /// <param name="attacker">Attacker</param>
    /// <param name="currentAttackFlags">Attack flags</param>
    /// <returns></returns>
    public bool IsCriticalAttack(Mover attacker, AttackFlags currentAttackFlags)
    {
        if (currentAttackFlags.HasFlag(AttackFlags.AF_MELEESKILL) || currentAttackFlags.HasFlag(AttackFlags.AF_MAGICSKILL))
        {
            return false;
        }

        int baseDexterity = attacker switch
        {
            Player p => p.Statistics.Dexterity,
            Monster m => m.Statistics.Dexterity,
            _ => attacker.Properties.Dexterity
        };
        var criticalJobFactor = attacker is Player player ? player.Job.Critical : 1f;
        var criticalProbability = (int)((baseDexterity + attacker.Attributes.Get(DefineAttributes.DST_DEX)) / 10 * criticalJobFactor);
        // TODO: add DST_CHR_CHANCECRITICAL to criticalProbability

        if (criticalProbability < 0)
        {
            criticalProbability = 0;
        }

        // TODO: check if player is in party and if it has the MVRF_CRITICAL flag

        return FFRandom.Random(0, 100) < criticalProbability;
    }

    /// <summary>
    /// Calculate critical damages.
    /// </summary>
    /// <param name="actualAttackRange">Attack result</param>
    public Range<int> CalculateCriticalDamages(Range<int> actualAttackRange)
    {
        var criticalMin = 1.1f;
        var criticalMax = 1.4f;

        if (Attacker.Level > Defender.Level)
        {
            if (Defender is Monster)
            {
                criticalMin = 1.2f;
                criticalMax = 2.0f;
            }
            else
            {
                criticalMin = 1.4f;
                criticalMax = 1.8f;
            }
        }

        float criticalBonus = 1; // TODO: 1 + (DST_CRITICAL_BONUS / 100)

        if (criticalBonus < 0.1f)
        {
            criticalBonus = 0.1f;
        }

        var attackMin = (int)(actualAttackRange.Minimum * criticalMin * criticalBonus);
        var attackMax = (int)(actualAttackRange.Maximum * criticalMax * criticalBonus);

        return new Range<int>(attackMin, attackMax);
    }

    /// <summary>
    /// Check if the current attack is a chance to knockback the defender.
    /// </summary>
    /// <param name="attackerAttackFlags">Attacker attack flags</param>
    /// <returns></returns>
    public bool IsKnockback(AttackFlags attackerAttackFlags)
    {
        var knockbackChance = FFRandom.Random(0, 100) < 15;

        if (Defender is Player)
        {
            return false;
        }

        if (Attacker is Player player)
        {
            Item weapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

            if (weapon.Properties.WeaponType == WeaponType.MELEE_YOYO || attackerAttackFlags.HasFlag(AttackFlags.AF_FORCE))
            {
                return false;
            }
        }

        var canFly = false;

        // TODO: if is flying, return false
        if (Defender.ObjectState.HasFlag(ObjectState.OBJSTA_DMG_FLY_ALL) && Defender is Monster monster)
        {
            canFly = monster.Properties.Class != MoverClassType.RANK_SUPER &&
                monster.Properties.Class != MoverClassType.RANK_MATERIAL &&
                monster.Properties.Class != MoverClassType.RANK_MIDBOSS;
        }

        return canFly && knockbackChance;
    }

    /// <summary>
    /// Gets range attack charge multiplier.
    /// </summary>
    /// <returns></returns>
    private float GetChargeAttackMultiplier()
    {
        if (!_attackFlags.HasFlag(AttackFlags.AF_RANGE))
        {
            return 1f;
        }

        return _attackPower switch
        {
            0 => 1.0f,
            1 => 1.2f,
            2 => 1.5f,
            3 => 1.8f,
            4 => 2.2f,
            _ => 1.0f
        };
    }
}
