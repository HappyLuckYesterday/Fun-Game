using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties.Quests;

public sealed class QuestProperties
{
    /// <summary>
    /// Gets the quest id.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the quest name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the quest title.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Gets the quest start character.
    /// </summary>
    public string StartCharacter { get; init; }

    /// <summary>
    /// Gets the quest end character.
    /// </summary>
    public string EndCharacter { get; init; }

    /// <summary>
    /// Gets the quest rewards.
    /// </summary>
    public QuestRewardProperties Rewards { get; init; }

    /// <summary>
    /// Gets the quest start requirements.
    /// </summary>
    public QuestStartRequirementsProperties StartRequirements { get; init; }

    /// <summary>
    /// Gets the quest ending condition.
    /// </summary>
    public QuestEndConditionProperties QuestEndCondition { get; init; }

    /// <summary>
    /// Gets the quest item drops.
    /// </summary>
    public IEnumerable<QuestItemDropProperties> Drops { get; init; }

    /// <summary>
    /// Gets the quest introduction/begin dialogs.
    /// </summary>
    public IEnumerable<string> BeginDialogs { get; init; }

    /// <summary>
    /// Gets the dialogs when the quest has been accepted by the player.
    /// </summary>
    public IEnumerable<string> AcceptedDialogs { get; init; }

    /// <summary>
    /// Gets the dialogs when the quest has been declined by the player.
    /// </summary>
    public IEnumerable<string> DeclinedDialogs { get; init; }

    /// <summary>
    /// Gets the dialogs when the quest is completed.
    /// </summary>
    public IEnumerable<string> CompletedDialogs { get; init; }

    /// <summary>
    /// Gets the dialogs when the quest is not finished.
    /// </summary>
    public IEnumerable<string> NotFinishedDialogs { get; init; }
}
