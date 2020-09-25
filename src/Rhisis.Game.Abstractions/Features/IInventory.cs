using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the player's inventory.
    /// </summary>
    public interface IInventory : IPacketSerializer, IEnumerable<IItem>
    {
        /// <summary>
        /// Gets the inventory max capacity.
        /// </summary>
        int MaxCapacity { get; }

        /// <summary>
        /// Sets the given collection of items into the inventory.
        /// </summary>
        /// <param name="items">Items to inject inside the inventory.</param>
        void SetItems(IEnumerable<IItem> items);

        /// <summary>
        /// Gets the first item matching the given predicate.
        /// </summary>
        /// <param name="predicate">Match predicate.</param>
        /// <returns>Item if found; null otherwise.</returns>
        IItem GetItem(Func<IItem, bool> predicate);

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        /// <param name="itemIndex">Item index.</param>
        /// <returns>Item at the given index; null otherwise.</returns>
        IItem GetItem(int itemIndex);

        /// <summary>
        /// Checks if the given item is equiped.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True if the item is equiped; false otherwise.</returns>
        bool IsItemEquiped(IItem item);

        /// <summary>
        /// Check if the given item is equipable by the current player.
        /// </summary>
        /// <param name="item">Item to equip.</param>
        /// <returns>True if the player can equip the item; false otherwise.</returns>
        bool IsItemEquipable(IItem item);

        /// <summary>
        /// Gets an equiped item based on a given item part.
        /// </summary>
        /// <param name="equipedItemPart">Item part type.</param>
        /// <returns>Equiped item if there is one; null otherwise.</returns>
        IItem GetEquipedItem(ItemPartType equipedItemPart);

        /// <summary>
        /// Gets the equiped items.
        /// </summary>
        /// <returns>Collection of equiped items.</returns>
        IEnumerable<IItem> GetEquipedItems();

        /// <summary>
        /// Equips an item to the given destination part.
        /// </summary>
        /// <param name="itemToEquip">Item to equip.</param>
        /// <returns>True if the item has been equiped; false otherwise.</returns>
        bool Equip(IItem itemToEquip);

        /// <summary>
        /// Unequip an item.
        /// </summary>
        /// <param name="itemToUnequip">Item to unequip.</param>
        /// <returns>True if the item has been unequiped; false otherwise.</returns>
        bool Unequip(IItem itemToUnequip);

        /// <summary>
        /// Moves an item from source slot to the destination slot.
        /// </summary>
        /// <param name="sourceSlot"></param>
        /// <param name="destinationSlot"></param>
        void Move(int sourceSlot, int destinationSlot);

        /// <summary>
        /// Deletes an item in player's inventory.
        /// </summary>
        /// <param name="itemIndex">Item index to delete.</param>
        /// <param name="quantity">Quantity to delete.</param>
        /// <param name="sendToPlayer">Sends the packet to the current player.</param>
        /// <returns>Deleted quantity.</returns>
        int DeleteItem(int itemIndex, int quantity, bool sendToPlayer = true);

        /// <summary>
        /// Deletes an item in player's inventory.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        /// <param name="quantity">Quantity to delete.</param>
        /// <param name="sendToPlayer">Sends the packet to the current player.</param>
        /// <returns>Deleted quantity.</returns>
        int DeleteItem(IItem item, int quantity, bool sendToPlayer = true);
    }
}
