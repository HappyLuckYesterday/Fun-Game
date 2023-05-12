using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties.Quests;

public sealed class QuestStartRequirementsProperties
{
    /// <summary>
    /// Gets the minimum level the player can start the quest.
    /// </summary>
    public int MinLevel { get; init; }

    /// <summary>
    /// Gets the maximum level the player can start the quest.
    /// </summary>
    public int MaxLevel { get; init; }

    /// <summary>
    /// Gets the jobs the player must have to start the quest.
    /// </summary>
    public IEnumerable<DefineJob.Job> Jobs { get; init; }

    /// <summary>
    /// Gets the previous quest id.
    /// </summary>
    public string PreviousQuestId { get; init; }
}
