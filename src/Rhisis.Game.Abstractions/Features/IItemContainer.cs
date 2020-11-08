using Rhisis.Game.Abstractions.Protocol;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to delay actions.
    /// </summary>
    public interface IDelayer : IDisposable
    {
        /// <summary>
        /// Delay an action using a time stamp has delay.
        /// </summary>
        /// <param name="delayTime">Delay time before executing action.</param>
        /// <param name="action">Action to execute after delay has passed.</param>
        /// <returns>Delayed action unique Id.</returns>
        Guid DelayAction(TimeSpan delayTime, Action action);

        /// <summary>
        /// Delay an action using seconds as time unit.
        /// </summary>
        /// <param name="delaySeconds">Delay time in seconds before executing action.</param>
        /// <param name="delayedAction">Action to execute after delay has passed.</param>
        /// <returns>Delayed action Id.</returns>
        Guid DelayAction(double delaySeconds, Action delayedAction);

        /// <summary>
        /// Delay an action using milliseconds as time unit.
        /// </summary>
        /// <param name="delayMilliseconds">Delay time in milliseconds before executing the action.</param>
        /// <param name="delayedAction">Action to execute after the delay has passed.</param>
        /// <returns>elayed action Id.</returns>
        Guid DelayActionMilliseconds(double delayMilliseconds, Action delayedAction);

        /// <summary>
        /// Cancels an action.
        /// </summary>
        /// <param name="delayedActionId">Delayed action id.</param>
        void CancelAction(Guid delayedActionId);

        /// <summary>
        /// Cancel all actions.
        /// </summary>
        void CancelAllActions();
    }

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
