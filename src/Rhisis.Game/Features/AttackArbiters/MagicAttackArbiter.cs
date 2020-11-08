using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class MagicAttackArbiter : AttackArbiterBase
    {
        private readonly int _magicPower;

        public MagicAttackArbiter(IMover attacker, IMover defender, int magicPower) 
            : base(attacker, defender)
        {
            _magicPower = magicPower;
        }

        public AttackResult CalculateDamages()
        {
            int damages = 0;

            if (Attacker is IPlayer player)
            {
                IItem wandWeapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? player.Inventory.Hand;

                Range<int> weaponAttackResult = GetWeaponAttackPower(player, wandWeapon);
                int weaponAttackDamages = GetWeaponAttackDamages(player, WeaponType.MAGIC_WAND);

                var attack = new Range<int>(weaponAttackResult.Minimum + weaponAttackDamages, weaponAttackResult.Maximum + weaponAttackDamages);

                damages = RandomHelper.Random(attack.Minimum, weaponAttackResult.Maximum);
                damages += Math.Max(0, Attacker.Attributes.Get(DefineAttributes.CHR_DMG));
                damages = (int)(damages * GetWandAttackMultiplier());
            }

            return AttackResult.Success(damages, AttackFlags.AF_MAGIC);
        }

        /// <summary>
        /// Gets the wand attack multiplier.
        /// </summary>
        /// <returns></returns>
        private float GetWandAttackMultiplier()
        {
            return _magicPower switch
            {
                0 => 0.6f,
                1 => 0.8f,
                2 => 1.05f,
                3 => 1.1f,
                4 => 1.3f,
                _ => 1.0f
            };
        }
    }
}
