using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Components
{
    /// <summary>
    /// Provides a mechanism to manage the player's inventory.
    /// </summary>
    public interface IInventory: IItemContainer
    {
        IEnumerable<IItem> GetEquipedItems();

        void Move(int sourceSlot, int destinationSlot);
    }
}
