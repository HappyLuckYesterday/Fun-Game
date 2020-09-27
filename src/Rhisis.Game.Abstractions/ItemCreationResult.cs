using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions
{
    public struct ItemCreationResult
    {
        public ItemCreationActionType ActionType { get; }

        public IItem Item { get; }

        public ItemCreationResult(ItemCreationActionType actionType, IItem item)
        {
            ActionType = actionType;
            Item = item;
        }
    }
}