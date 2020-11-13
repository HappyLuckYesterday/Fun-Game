using Rhisis.Game.Abstractions.Protocol;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage an item container.
    /// </summary>
    public interface IItemContainer : IPacketSerializer, IEnumerable<IItem>
    {
        /// <summary>
        /// Gets the total amount of items in this container.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the storage capacity.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Gets the extra storage capacity.
        /// </summary>
        int ExtraCapacity { get; }

        /// <summary>
        /// Gets the maximum capacity of the container.
        /// </summary>
        int MaxCapacity { get; }

        /// <summary>
        /// Gets the items slots mask.
        /// </summary>
        /// <remarks>
        /// The masks actually contain the item indexes in the main array.
        /// For example, at slot 1, we could find the item stored at index 28 in the internal item list.
        /// Once you have this index, use the <see cref="this[int]"/> indexer or the <see cref="GetItemAtIndex(int)"/> method to retrieve the item.
        /// </remarks>
        int[] Masks { get; }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item at index if found; throws a <see cref="IndexOutOfRangeException"/> otherwise.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
        IItem this[int index] { get; }

        /// <summary>
        /// Initialize the container items.
        /// </summary>
        /// <param name="items">Items to store in the container.</param>
        void Initialize(IEnumerable<IItem> items);

        /// <summary>
        /// Gets an item matching the given predicate function.
        /// </summary>
        /// <param name="predicate">Matching predicate.</param>
        /// <returns>The item matching the predicate if found; null otherwise.</returns>
        IItem GetItem(Func<IItem, bool> predicate);

        /// <summary>
        /// Gets the first item by its id.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>The item matching the id; null otherwise.</returns>
        IItem GetItem(int itemId);

        /// <summary>
        /// Gets the item at a given slot.
        /// </summary>
        /// <param name="slot">Slot.</param>
        /// <returns>The item if found; null otherwise.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the slot is out of range.</exception>
        IItem GetItemAtSlot(int slot);

        /// <summary>
        /// Gets the item at a given index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item at index if found; throws a null otherwise.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
        IItem GetItemAtIndex(int index);

        /// <summary>
        /// Gets a range of items from a given start point.
        /// </summary>
        /// <param name="start">Start index.</param>
        /// <param name="count">Item amount.</param>
        /// <returns>Collection of items in the range.</returns>
        IEnumerable<IItem> GetRange(int start, int count);

        /// <summary>
        /// Check if the item container can store the given item.
        /// </summary>
        /// <param name="itemToStore">Item to store.</param>
        /// <returns>True if the item container can store the item; false otherwise.</returns>
        bool CanStoreItem(IItem itemToStore);

        /// <summary>
        /// Creates an item to the item container.
        /// </summary>
        /// <param name="item">Item to add.</param>
        IEnumerable<ItemCreationResult> CreateItem(IItem item);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        void DeleteItem(IItem item);

        /// <summary>
        /// Swap two items.
        /// </summary>
        /// <param name="sourceSlot">Source slot.</param>
        /// <param name="destinationSlot">Destination slot.</param>
        void SwapItem(int sourceSlot, int destinationSlot);
    }
}
