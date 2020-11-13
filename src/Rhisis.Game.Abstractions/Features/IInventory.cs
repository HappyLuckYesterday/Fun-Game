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
        /// Gets the inventory storage capacity.
        /// </summary>
        int StorageCapacity { get; }

        /// <summary>
        /// Gets the inventory max capacity.
        /// </summary>
        int MaxCapacity { get; }

        /// <summary>
        /// Gets the player's hand item.
        /// </summary>
        IItem Hand { get; }

        /// <summary>
        /// Gets or sets the delayed action id of the current item behing used.
        /// </summary>
        Guid ItemInUseActionId { get; set; }

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
        /// Gets the number of items of the inventory.
        /// </summary>
        /// <returns></returns>
        int GetItemCount();

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
        void MoveItem(int sourceSlot, int destinationSlot);

        /// <summary>
        /// Creates an item in player's inventory.
        /// </summary>
        /// <param name="item">Item model to create.</param>
        /// <param name="quantity">Quantity to create.</param>
        /// <param name="creatorId">Id of the character that created the item. Used for GMs and admins.</param>
        /// <param name="sendToPlayer">Sends the packet to the current player.</param>
        /// <returns>Number of items created.</returns>
        int CreateItem(IItem item, int quantity, int creatorId = -1, bool sendToPlayer = true);

        /// <summary>
        /// Deletes an item in player's inventory.
        /// </summary>
        /// <param name="itemIndex">Item index to delete.</param>
        /// <param name="quantity">Quantity to delete.</param>
        /// <param name="updateType">Update item type.</param>
        /// <param name="sendToPlayer">Sends the packet to the current player.</param>
        /// <returns>Deleted quantity.</returns>
        int DeleteItem(int itemIndex, int quantity, UpdateItemType updateType = UpdateItemType.UI_NUM, bool sendToPlayer = true);

        /// <summary>
        /// Deletes an item in player's inventory.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        /// <param name="quantity">Quantity to delete.</param>
        /// <param name="updateType">Update item type.</param>
        /// <param name="sendToPlayer">Sends the packet to the current player.</param>
        /// <returns>Deleted quantity.</returns>
        int DeleteItem(IItem item, int quantity, UpdateItemType updateType = UpdateItemType.UI_NUM, bool sendToPlayer = true);

        /// <summary>
        /// Uses an inventory item.
        /// </summary>
        /// <param name="item">Inventory item.</param>
        void UseItem(IItem item);

        /// <summary>
        /// Checks if the item has a cooltime.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ItemHasCoolTime(IItem item);

        /// <summary>
        /// Check if the given item is a cooltime item and can be used.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns true if the item with cooltime can be used; false otherwise.</returns>
        bool CanUseItemWithCoolTime(IItem item);

        /// <summary>
        /// Sets item cool time.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="cooltime">Cooltime.</param>
        void SetCoolTime(IItem item, int cooltime);
    }
}
