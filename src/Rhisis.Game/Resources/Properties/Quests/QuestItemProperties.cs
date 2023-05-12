using Rhisis.Game.Common;

namespace Rhisis.Game.Resources.Properties.Quests;

public sealed class QuestItemProperties
{
    /// <summary>
    /// Gets or sets the quest item id.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets or sets the quest item quantity.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets or sets the quest item sex.
    /// </summary>
    public GenderType Sex { get; init; }

    /// <summary>
    /// Gets or sets the item refine.
    /// </summary>
    public byte Refine { get; init; }

    /// <summary>
    /// Gets or sets the item element.
    /// </summary>
    public ElementType Element { get; init; }

    /// <summary>
    /// Gets or sets the item element refine.
    /// </summary>
    public byte ElementRefine { get; init; }

    /// <summary>
    /// Gets or sets a value that indicates if this item has to be removed from the inventory
    /// once the quest is completed.
    /// </summary>
    public bool Remove { get; init; }
}
