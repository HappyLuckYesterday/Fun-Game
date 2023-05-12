namespace Rhisis.Game.Resources.Properties.Quests;

public class QuestItemDropProperties
{
    /// <summary>
    /// Gets or sets the monster id where the item can be dropped.
    /// </summary>
    public string MonsterId { get; init; }

    /// <summary>
    /// Gets or sets the item id that can be dropped.
    /// </summary>
    public string ItemId { get; init; }

    /// <summary>
    /// Gets or sets the drop probability for the specified monster.
    /// </summary>
    public long Probability { get; init; }

    /// <summary>
    /// Gets or sets the item drop quantity.
    /// </summary>
    public int Quantity { get; init; }
}