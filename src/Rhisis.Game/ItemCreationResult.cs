namespace Rhisis.Game;

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
    public Item Item { get; }

    /// <summary>
    /// Gets the item slot.
    /// </summary>
    public int Slot { get; }

    /// <summary>
    /// Gets the item index.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Creates a new <see cref="ItemCreationResult"/> instance.
    /// </summary>
    /// <param name="actionType">Item action type.</param>
    /// <param name="item">Item.</param>
    /// <param name="slot">Item slot in container.</param>
    /// <param name="index">Item index in container.</param>
    public ItemCreationResult(ItemCreationActionType actionType, Item item, int slot, int index)
    {
        ActionType = actionType;
        Item = item;
        Slot = slot;
        Index = index;
    }
}