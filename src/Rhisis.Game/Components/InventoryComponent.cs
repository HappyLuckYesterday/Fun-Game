using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Abstractions.Components
{
    public class InventoryComponent : ItemContainerComponent<Item>, IInventory
    {
        public const int InventorySize = 42;
        public const int InventoryEquipParts = 31;
        private readonly IPlayer _player;

        public InventoryComponent(IPlayer player)
            : base(InventorySize, InventoryEquipParts)
        {
            _player = player;
        }

        public IEnumerable<IItem> GetEquipedItems()
        {
            return _itemsMask.GetRange(InventorySize, InventoryEquipParts).Select(x => _items.ElementAtOrDefault(x));
        }

        public void Move(int sourceSlot, int destinationSlot)
        {
            throw new NotImplementedException();
        }
    }
}
