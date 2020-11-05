using Rhisis.Game.Abstractions.Protocol;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    public interface IItemContainer : IPacketSerializer, IEnumerable<IItem>
    {
        int Count { get; }

        int Capacity { get; }

        int ExtraCapacity { get; }

        int MaxCapacity { get; }

        int[] Masks { get; }

        IItem this[int index] { get; }

        void Initialize(IEnumerable<IItem> items);

        IItem GetItem(Func<IItem, bool> predicate);

        IItem GetItem(int itemId);

        IItem GetItemAtSlot(int slot);

        IItem GetItemAtIndex(int index);

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

        void DeleteItem(IItem item);

        void SwapItem(int sourceSlot, int destinationSlot);
    }
}
