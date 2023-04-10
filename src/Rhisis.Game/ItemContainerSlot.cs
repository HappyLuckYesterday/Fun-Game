namespace Rhisis.Game;

/// <summary>
/// Describes an item slot of an item container.
/// </summary>
public class ItemContainerSlot
{
    /// <summary>
    /// Gets or sets the slot index.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the slot number.
    /// </summary>
    public int Slot { get; set; }

    /// <summary>
    /// Gets or sets the item on the current slot.
    /// </summary>
    public Item Item { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the slot has an item.
    /// </summary>
    public bool HasItem => Item is not null;
}