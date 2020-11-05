using Rhisis.Core.Helpers;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Features.AttackArbiters.Reducers
{
    public abstract class AttackReducerBase
    {
        /// <summary>
        /// Gets the attacker.
        /// </summary>
        protected IMover Attacker { get; }
        
        /// <summary>
        /// Gets the defender.
        /// </summary>
        protected IMover Defender { get; }

        /// <summary>
        /// Creates and initializes a new attack reducer.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        protected AttackReducerBase(IMover attacker, IMover defender)
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
        /// Gets the defender defense multiplier.
        /// </summary>
        /// <param name="defenseFactor">Defense factor. (default is 1.0f)</param>
        /// <returns></returns>
        protected virtual float GetDefenseMultiplier(float defenseFactor = 1.0f)
        {
            defenseFactor *= 1.0f + Defender.Attributes.Get(DefineAttributes.ADJDEF_RATE) / 100.0f;

            return defenseFactor;
        }

        /// <summary>
        /// Gets the defender blocking factor.
        /// </summary>
        /// <returns>Defender blocking factor</returns>
        protected float GetDefenderBlockFactor()
        {
            if (Defender is IPlayer player)
            {
                var blockRandomProbability = RandomHelper.Random(0, 80);

                if (blockRandomProbability <= 5)
                    return 1f;
                if (blockRandomProbability >= 75f)
                    return 0.1f;

                var defenderLevel = Defender.Level;
                var defenderDexterity = Defender.Attributes.Get(DefineAttributes.DEX);

                var blockProbabilityA = defenderLevel / ((defenderLevel + Attacker.Level) * 15.0f);
                var blockProbabilityB = (defenderDexterity + Attacker.Attributes.Get(DefineAttributes.DEX) + 2) * ((defenderDexterity - Attacker.Attributes.Get(DefineAttributes.DEX)) / 800.0f);

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
                    return 0f;
            }
            else if (Defender is IMonster)
            {
                var blockRandomProbability = RandomHelper.Random(0, 100);

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
        protected int GetEspaceRating(IMover mover)
        {
            return mover switch
            {
                IPlayer player => (int)((player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DEX) + player.Attributes.Get(DefineAttributes.PARRY)) * 0.5f),
                IMonster monster => monster.Data.EscapeRating,
                _ => 0,
            };
        }
    }
}
