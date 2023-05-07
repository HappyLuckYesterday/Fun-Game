using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Battle.AttackArbiters.Reducers;

public abstract class AttackReducerBase
{
    /// <summary>
    /// Gets the attacker.
    /// </summary>
    protected Mover Attacker { get; }

    /// <summary>
    /// Gets the defender.
    /// </summary>
    protected Mover Defender { get; }

    /// <summary>
    /// Creates and initializes a new attack reducer.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    protected AttackReducerBase(Mover attacker, Mover defender)
    {
        Attacker = attacker;
        Defender = defender;
    }

    /// <summary>
    /// Reduces the damages.
    /// </summary>
    /// <param name="attackResult">Attack result previously calculated using an attack arbiter.</param>
    /// <returns></returns>
    public abstract AttackResult ReduceDamages(AttackResult attackResult);

    /// <summary>
    /// Gets the defender defense.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns>Defender defense.</returns>
    protected int GetDefenderDefense(AttackFlags flags)
    {
        var defense = 0;
        bool isGenericAttack = flags.HasFlag(AttackFlags.AF_GENERIC) || Attacker is Player && Defender is Player;

        int level = Defender.Level;

        if (isGenericAttack)
        {
            float defenseFactor = Defender is Player player ? player.Job.DefenseFactor : 1f;
            int armorDefense = Defender.Defense.Get();
            int stamina = Defender.Statistics.Stamina + Defender.Attributes.Get(DefineAttributes.DST_STA);

            defense = (int)((level * 2 + stamina / 2) / 2.8f - 4 + (stamina - 14) * defenseFactor);
            defense += armorDefense / 4;
            defense += Defender.Attributes.Get(DefineAttributes.DST_ADJDEF);

            defense = Math.Max(defense, 0);
        }
        else if (Defender is Monster monster)
        {
            int monsterDefenseArmor = flags.HasFlag(AttackFlags.AF_MAGIC) ? monster.Properties.MagicResitance : monster.Properties.NaturalArmor;

            defense = (int)(monsterDefenseArmor / 7f + 1);
        }
        else if (Defender is Player)
        {
            int stamina = Defender.Statistics.Stamina + Defender.Attributes.Get(DefineAttributes.DST_STA);
            int dexterity = Defender.Statistics.Dexterity + Defender.Attributes.Get(DefineAttributes.DST_DEX);

            if (Attacker is Player)
            {
                if (flags.HasFlag(AttackFlags.AF_MAGIC))
                {
                    defense = (int)(Defender.Attributes.Get(DefineAttributes.DST_INT) * 9.04f + level * 35.98f);
                }
                else
                {
                    int armorDefense = Defender.Defense.Get() + Defender.Attributes.Get(DefineAttributes.DST_ADJDEF);

                    defense = (int)(armorDefense * 2.3f + (level + stamina / 2 + dexterity) / 2.8f - 4 + level * 2);
                }
            }
            else
            {
                int armorDefense = Defender.Defense.Get() / 4 + Defender.Attributes.Get(DefineAttributes.DST_ADJDEF);

                defense = (int)(armorDefense + (level + stamina / 2 + dexterity) / 2.8f - 4 + level * 2);
            }

            defense = Math.Max(defense, 0);
        }

        return (int)(defense * GetDefenseMultiplier());
    }

    /// <summary>
    /// Gets the defender defense multiplier.
    /// </summary>
    /// <param name="defenseFactor">Defense factor. (default is 1.0f)</param>
    /// <returns></returns>
    protected virtual float GetDefenseMultiplier(float defenseFactor = 1.0f)
    {
        defenseFactor *= 1.0f + Defender.Attributes.Get(DefineAttributes.DST_ADJDEF_RATE) / 100.0f;

        return defenseFactor;
    }

    /// <summary>
    /// Gets the defender blocking factor.
    /// </summary>
    /// <returns>Defender blocking factor</returns>
    protected float GetDefenderBlockFactor()
    {
        if (Defender is Player player)
        {
            int blockRandomProbability = FFRandom.Random(0, 80);

            if (blockRandomProbability <= 5)
            {
                return 1f;
            }

            if (blockRandomProbability >= 75)
            {
                return 0.1f;
            }

            var defenderLevel = Defender.Level;
            var defenderDexterity = Defender.Attributes.Get(DefineAttributes.DST_DEX);

            var blockProbabilityA = defenderLevel / ((defenderLevel + Attacker.Level) * 15.0f);
            var blockProbabilityB = (defenderDexterity + Attacker.Attributes.Get(DefineAttributes.DST_DEX) + 2) * ((defenderDexterity - Attacker.Attributes.Get(DefineAttributes.DST_DEX)) / 800.0f);

            if (blockProbabilityB > 10.0f)
            {
                blockProbabilityB = 10.0f;
            }

            var blockProbability = blockProbabilityA + blockProbabilityB;
            if (blockProbability < 0.0f)
            {
                blockProbability = 0.0f;
            }

            // TODO: range attack probability

            var blockRate = (int)(defenderDexterity / 8.0f * player.Job.Blocking + blockProbability);

            if (blockRate < 0)
            {
                blockRate = 0;
            }

            if (blockRate > blockRandomProbability)
            {
                return 0f;
            }
        }
        else if (Defender is Monster)
        {
            int blockRandomProbability = FFRandom.Random(0, 100);

            if (blockRandomProbability <= 5)
            {
                return 1f;
            }

            if (blockRandomProbability >= 95)
            {
                return 0f;
            }

            var blockRate = (int)((GetEspaceRating(Defender) - Defender.Level) * 0.5f);

            if (blockRate < 0)
            {
                blockRate = 0;
            }

            if (blockRate > blockRandomProbability)
            {
                return 0.2f;
            }
        }

        return 1f;
    }

    /// <summary>
    /// Gets the escape rating of an entity.
    /// </summary>
    /// <param name="mover"></param>
    /// <returns></returns>
    protected int GetEspaceRating(Mover mover)
    {
        return mover switch
        {
            Player player => (int)((player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DST_DEX) + player.Attributes.Get(DefineAttributes.DST_PARRY)) * 0.5f),
            Monster monster => monster.Properties.EscapeRating,
            _ => 0,
        };
    }
}
