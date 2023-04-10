using System.Diagnostics;

namespace Rhisis.Game;

/// <summary>
/// Describes an item slot of an item container.
/// </summary>
[DebuggerDisplay("Slot = {Slot} (Index = {Index}) | Item = {HasItem ? Item.Name : \"none\"}")]
public class ItemContainerSlot
{
    public static readonly ItemContainerSlot Empty = new ItemContainerSlot()
    {
        Index = -1,
        Slot = -1,
        Item = null
    };

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