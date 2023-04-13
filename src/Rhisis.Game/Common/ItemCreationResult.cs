using System;

namespace Rhisis.Game.Common;

/// <summary>
/// Represents the result of an item creation.
/// </summary>
public readonly struct ItemCreationResult : IEquatable<ItemCreationResult>
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

    /// <summary>
    /// Compares two <see cref="ItemCreationResult"/> objects.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if both are the same; false otherwise.</returns>
    public bool Equals(ItemCreationResult other) 
        => Slot == other.Slot && Index == other.Index && ActionType == other.ActionType && Item == other.Item;

    /// <summary>
    /// Compares the current <see cref="ItemCreationResult"/> with another object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True if both are the same; false otherwise.</returns>
    public override bool Equals(object obj) => obj is ItemCreationResult result && Equals(result);

    /// <summary>
    /// Gets the hashcode of the current <see cref="ItemCreationResult"/>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(ActionType, Item, Slot, Index);
}