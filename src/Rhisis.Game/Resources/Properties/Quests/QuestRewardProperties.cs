using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties.Quests;

public sealed class QuestRewardProperties
{
    /// <summary>
    /// Gets the quest gold reward.
    /// </summary>
    public int Gold { get; }

    /// <summary>
    /// Gets the quest experience reward.
    /// </summary>
    public long Experience { get; }

    /// <summary>
    /// Gets the quest items reward.
    /// </summary>
    public IEnumerable<QuestItemProperties> Items { get; }

    /// <summary>
    /// Gets a boolean value that indiciates if the system should restat the player.
    /// </summary>
    public bool Restat { get; }

    /// <summary>
    /// Gets a boolean value that indiciates if the systme should reskill the player.
    /// </summary>
    public bool Reskill { get; }

    /// <summary>
    /// Gets the number of skills points to offer to the player.
    /// </summary>
    public ushort SkillPoints { get; }

    /// <summary>
    /// Gets the reward job based on the player information.
    /// </summary>
    /// <param name="player">Current player entity information.</param>
    /// <returns></returns>
    public DefineJob.Job GetJob(object player)
    {
        return DefineJob.Job.JOB_ELEMENTOR_HERO;
    }

    /// <summary>
    /// Checks if the quest has a job reward.
    /// </summary>
    /// <returns>True if the quest has job reward; false otherwise.</returns>
    public bool HasJobReward() 
    {
        return false;
    }
}
