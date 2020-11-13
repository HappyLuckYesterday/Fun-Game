using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions
{
    /// <summary>
    /// Represents the result of an item creation.
    /// </summary>
    public struct ItemCreationResult
    {
        /// <summary>
        /// Gets the item action type.
        /// </summary>
        public ItemCreationActionType ActionType { get; }

        /// <summary>
        /// Gets the item where the action occured.
        /// </summary>
        public IItem Item { get; }

        /// <summary>
        /// Creates a new <see cref="ItemCreationResult"/> instance.
        /// </summary>
        /// <param name="actionType">Item action type.</param>
        /// <param name="item">Item.</param>
        public ItemCreationResult(ItemCreationActionType actionType, IItem item)
        {
            ActionType = actionType;
            Item = item;
        }
    }
}