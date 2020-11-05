using Rhisis.Core.Helpers;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class Defense : GameFeature, IDefense
    {
        private readonly IMover _mover;

        public int Minimum { get; private set; }

        public int Maximum { get; private set; }

        public Defense(IMover mover)
        {
            _mover = mover;
        }

        public int GetDefense()
        {
            if (Minimum == Maximum)
            {
                return Maximum;
            }

            int defenseDelta = Maximum - Minimum;

            return Minimum + (defenseDelta > 0 ? RandomHelper.Random(0, defenseDelta) : 0);
        }

        public void Update()
        {
            if (_mover is IPlayer player)
            {
                var defenseMin = 0;
                var defenseMax = 0;
                IEnumerable<IItem> equipedItems = player.Inventory.GetEquipedItems();

                if (equipedItems.Any())
                {
                    foreach (Item equipedItem in equipedItems)
                    {
                        if (equipedItem == null || (equipedItem != null && equipedItem.Id == -1))
                        {
                            continue;
                        }

                        if (equipedItem.Data.ItemKind2 == ItemKind2.ARMOR || equipedItem.Data.ItemKind2 == ItemKind2.ARMORETC)
                        {
                            int refineValue = equipedItem.Refine > 0 ? (int)Math.Pow(equipedItem.Refine, 1.5f) : 0;
                            const float itemMultiplier = 1; // TODO: implement GetItemMultiplier() on the Item class of the Rhisis Domain.

                            defenseMin += (int)(equipedItem.Data.AbilityMin * itemMultiplier) + refineValue;
                            defenseMax += (int)(equipedItem.Data.AbilityMax * itemMultiplier) + refineValue;
                        }
                    }
                }

                defenseMin += player.Attributes.Get(DefineAttributes.ABILITY_MIN);
                defenseMax += player.Attributes.Get(DefineAttributes.ABILITY_MAX);
                Minimum = defenseMin;
                Maximum = defenseMax;
            }
            else if (_mover is IMonster monster)
            {
                Minimum = monster.Data.NaturalArmor;
                Minimum = monster.Data.NaturalArmor;
            }
        }
    }
}
