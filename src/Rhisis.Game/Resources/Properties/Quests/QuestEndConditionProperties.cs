using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties.Quests;

public sealed class QuestEndConditionProperties
{
    /// <summary>
    /// Gets the items description to complete the quest.
    /// </summary>
    public IEnumerable<QuestItemProperties> Items { get; init; }

    /// <summary>
    /// Gets the monsters information to kill to complete the quest.
    /// </summary>
    public IEnumerable<QuestMonsterProperties> Monsters { get; init; }

    /// <summary>
    /// Gets the patrols to be done to complete the quest.
    /// </summary>
    public IEnumerable<QuestPatrolProperties> Patrols { get; init; }
}
