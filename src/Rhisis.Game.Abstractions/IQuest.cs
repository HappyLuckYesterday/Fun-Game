using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common.Resources.Quests;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions
{
    public interface IQuest : IPacketSerializer
    {
        /// <summary>
        /// Gets the quest id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the quest database id.
        /// </summary>
        int? DatabaseQuestId { get; }

        /// <summary>
        /// Gets the quest character id.
        /// </summary>
        int CharacterId { get; }

        /// <summary>
        /// Gets or sets a boolean value that indiciates the quest checked in state in the player's interface.
        /// </summary>
        bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the quest has been deleted or not.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the quest is finished or not.
        /// </summary>
        bool IsFinished { get; set; }
        
        /// <summary>
        /// Gets or sets a boolean value that indicates if the patrol has been done for the current quest.
        /// </summary>
        bool IsPatrolDone { get; set; }

        /// <summary>
        /// Gets or sets the quest state.
        /// </summary>
        QuestStateType State { get; set; }

        /// <summary>
        /// Gets the quest start time.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets a dictionary of the killed monsters. Key is the monster id; Value is the killed amount.
        /// </summary>
        IDictionary<int, short> Monsters { get; set; }

        /// <summary>
        /// Gets the quest script.
        /// </summary>
        IQuestScript Script { get; }
    }
}
