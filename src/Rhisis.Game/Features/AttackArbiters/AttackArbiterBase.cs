using Rhisis.Core.Common;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class AttackArbiterBase
    {
        public IMover Attacker { get; }
        
        public IMover Defender { get; }

        protected AttackArbiterBase(IMover attacker, IMover defender)
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
        public int GetEspaceRating(IMover entity)
        {
            if (entity is IPlayer player)
            {
                int playerDexterity = player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DEX);

                return (int)(playerDexterity * 0.5f); // TODO: add DST_PARRY
            }
            else if (entity is IMonster monster)
            {
                return monster.Data.EscapeRating;
            }

            return default;
        }

        /// <summary>
        /// Gets the weapon attack damages based on player's statistics.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="weaponType">Weapon type</param>
        /// <returns></returns>
        public int GetWeaponAttackDamages(IPlayer player, WeaponType weaponType)
        {
            float attribute = 0f;
            float levelFactor = 0f;
            float jobFactor = 1f;

            switch (weaponType)
            {
                case WeaponType.MELEE_SWD:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 12;
                    levelFactor = player.Level * 1.1f;
                    jobFactor = player.Job.MeleeSword;
                    break;
                case WeaponType.MELEE_AXE:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 12;
                    levelFactor = player.Level * 1.2f;
                    jobFactor = player.Job.MeleeAxe;
                    break;
                case WeaponType.MELEE_STAFF:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.1f;
                    jobFactor = player.Job.MeleeStaff;
                    break;
                case WeaponType.MELEE_STICK:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.3f;
                    jobFactor = player.Job.MeleeStick;
                    break;
                case WeaponType.MELEE_KNUCKLE:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.2f;
                    jobFactor = player.Job.MeleeKnuckle;
                    break;
                case WeaponType.MAGIC_WAND:
                    attribute = player.Statistics.Intelligence + player.Attributes.Get(DefineAttributes.INT) - 10;
                    levelFactor = player.Level * 1.2f;
                    jobFactor = player.Job.MagicWand;
                    break;
                case WeaponType.MELEE_YOYO:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.1f;
                    jobFactor = player.Job.MeleeYoyo;
                    break;
                case WeaponType.RANGE_BOW:
                    attribute = ((player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DEX)) - 14) * 4f;
                    levelFactor = player.Level * 1.3f;
                    jobFactor = ((player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR)) * 0.2f) * 0.7f;
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
        public Range<int> GetWeaponAttackPower(IMover entity, IItem weapon)
        {
            float multiplier = GetWeaponItemMultiplier(weapon);
            int power = weapon?.Refine > 0 ? (int)Math.Pow(weapon?.Refine ?? default, 1.5f) : default;

            return new Range<int>(
                (int)((entity.Attributes.Get(DefineAttributes.ABILITY_MIN) + weapon?.Data.AbilityMin) * multiplier) + power,
                (int)((entity.Attributes.Get(DefineAttributes.ABILITY_MAX) + weapon?.Data.AbilityMax) * multiplier) + power
            );
        }

        /// <summary>
        /// Gets the weapon item multiplier.
        /// </summary>
        /// <param name="weapon">Current used weapon.</param>
        /// <returns></returns>
        public float GetWeaponItemMultiplier(IItem weapon)
        {
            if (weapon is null)
            {
                return 1f;
            }

            // TODO: check if item has expired.
            float multiplier = 1.0f;
            int refine = weapon.Data.WeaponKind == WeaponKindType.Ultimate ? ItemConstants.WeaponArmonRefineMax : weapon.Refine;

            if (refine > 0)
            {
                // TODO: get item exp up
                int itemExpUp = 0 + 100;
                multiplier *= (itemExpUp / 100);
            }

            return multiplier;
        }

        /// <summary>
        /// Gets the weapon extra damages based weapon type and entity attribute bonuses.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        /// <param name="weapon">Current entity weapon.</param>
        /// <returns>Weapon extra damages</returns>
        public int GetWeaponExtraDamages(IMover entity, IItem weapon)
        {
            if (weapon is null)
            {
                return default;
            }

            int extraDamages = weapon.Data.WeaponType switch
            {
                WeaponType.MELEE_SWD => entity.Attributes.Get(DefineAttributes.SWD_DMG) + entity.Attributes.Get(DefineAttributes.TWOHANDMASTER_DMG),
                WeaponType.MELEE_AXE => entity.Attributes.Get(DefineAttributes.AXE_DMG) + entity.Attributes.Get(DefineAttributes.TWOHANDMASTER_DMG),
                WeaponType.KNUCKLE => entity.Attributes.Get(DefineAttributes.KNUCKLE_DMG) + entity.Attributes.Get(DefineAttributes.KNUCKLEMASTER_DMG),
                WeaponType.MELEE_YOYO => entity.Attributes.Get(DefineAttributes.YOY_DMG) + entity.Attributes.Get(DefineAttributes.YOYOMASTER_DMG),
                WeaponType.RANGE_BOW => entity.Attributes.Get(DefineAttributes.BOW_DMG) + entity.Attributes.Get(DefineAttributes.BOWMASTER_DMG),
                _ => default
            };

            if (entity is IPlayer player)
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
            var multiplier = 1.0f + Attacker.Attributes.Get(DefineAttributes.ATKPOWER_RATE) / 100f;

            if (Attacker is IPlayer)
            {
                // TODO: check SM mode SM_ATTACK_UP or SM_ATTACK_UP1 => multiplier *= 1.2f;

                var attribute = Defender is IPlayer ? DefineAttributes.PVP_DMG : DefineAttributes.MONSTER_DMG;
                int damages = Attacker.Attributes.Get(attribute);

                if (damages > 0)
                {
                    multiplier += multiplier * damages / 100;
                }
            }

            return multiplier;
        }
    }
}
