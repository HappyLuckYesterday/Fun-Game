using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;
using Rhisis.Core.Helpers;
using System.Collections.Generic;

namespace Rhisis.Game.Battle.AttackArbiters;

public sealed class MagicAttackArbiter : AttackArbiterBase
{
    private static readonly Dictionary<int, float> _wandAttackMultiplier = new()
    {
        { 0, 0.6f },
        { 1, 0.8f },
        { 2, 1.05f },
        { 3, 1.1f },
        { 4, 1.3f }
    };

    private readonly int _magicPower;

    public MagicAttackArbiter(Mover attacker, Mover defender, int magicPower) 
        : base(attacker, defender)
    {
        _magicPower = magicPower;
    }

    public override AttackResult CalculateDamages()
    {
        int damages = 0;

        if (Attacker is Player player)
        {
            Item wandWeapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);
            Range<int> weaponAttackResult = GetWeaponAttackPower(player, wandWeapon);
            int weaponAttackDamages = GetWeaponAttackDamages(player, WeaponType.MAGIC_WAND);
            Range<int> attack = new(weaponAttackResult.Minimum + weaponAttackDamages, weaponAttackResult.Maximum + weaponAttackDamages);

            damages = FFRandom.Random(attack.Minimum, weaponAttackResult.Maximum);
            damages += Math.Max(0, Attacker.Attributes.Get(DefineAttributes.DST_CHR_DMG));
            damages = (int)(damages * _wandAttackMultiplier.GetValueOrDefault(_magicPower, 1.0f));
        }

        return AttackResult.Success(damages, AttackFlags.AF_MAGIC);
    }
}