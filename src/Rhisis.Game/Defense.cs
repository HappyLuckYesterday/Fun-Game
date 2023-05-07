using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class Defense
{
    private readonly Mover _mover;

    public int Minimum { get; private set; }

    public int Maximum { get; private set; }

    public Defense(Mover mover)
    {
        _mover = mover;
    }

    public int Get()
    {
        if (Minimum == Maximum)
        {
            return Maximum;
        }

        int defenseDelta = Maximum - Minimum;

        return Minimum + (defenseDelta > 0 ? FFRandom.Random(0, defenseDelta) : 0);
    }

    public void Update()
    {
        if (_mover is Player player)
        {
            var defenseMin = 0;
            var defenseMax = 0;
            IEnumerable<Item> equipedItems = player.GetEquipedItems();

            if (equipedItems.Any())
            {
                foreach (Item equipedItem in equipedItems)
                {
                    if (equipedItem == null || equipedItem != null && equipedItem.Id == -1)
                    {
                        continue;
                    }

                    if (equipedItem.Properties.ItemKind2 == ItemKind2.ARMOR || equipedItem.Properties.ItemKind2 == ItemKind2.ARMORETC)
                    {
                        int refineValue = equipedItem.Refine > 0 ? (int)Math.Pow(equipedItem.Refine, 1.5f) : 0;
                        const float itemMultiplier = 1; // TODO: implement GetItemMultiplier() on the Item class of the Rhisis Domain.

                        defenseMin += (int)(equipedItem.Properties.AbilityMin * itemMultiplier) + refineValue;
                        defenseMax += (int)(equipedItem.Properties.AbilityMax * itemMultiplier) + refineValue;
                    }
                }
            }

            defenseMin += player.Attributes.Get(DefineAttributes.DST_ABILITY_MIN);
            defenseMax += player.Attributes.Get(DefineAttributes.DST_ABILITY_MAX);
            Minimum = defenseMin;
            Maximum = defenseMax;
        }
        else if (_mover is Monster monster)
        {
            Minimum = monster.Properties.NaturalArmor;
            Minimum = monster.Properties.NaturalArmor;
        }
    }
}
