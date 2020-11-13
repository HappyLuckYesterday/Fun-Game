using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common.Resources.Quests;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the player quest diary.
    /// </summary>
    public interface IQuestDiary : IPacketSerializer, IEnumerable<IQuest>
    {
        /// <summary>
        /// Gets the active quests.
        /// </summary>
        IEnumerable<IQuest> ActiveQuests { get; }

        /// <summary>
        /// Gets the checked quests.
        /// </summary>
        IEnumerable<IQuest> CheckedQuests { get; }

        /// <summary>
        /// Gets the completed quests.
        /// </summary>
        IEnumerable<IQuest> CompletedQuests { get; }

        /// <summary>
        /// Gets the active quest based on the given id.
        /// </summary>
        /// <param name="questId">Quest id.</param>
        /// <returns><see cref="IQuest"/>.</returns>
        IQuest GetActiveQuest(int questId);

        /// <summary>
        /// Checks if the quest exists in the diary.
        /// </summary>
        /// <param name="questId">Quest Id.</param>
        /// <returns>True if the quest exists, false otherwise.</returns>
        bool HasQuest(int questId);

        /// <summary>
        /// Adds a new quest to the diary.
        /// </summary>
        /// <param name="quest"></param>
        void Add(IQuest quest);

        /// <summary>
        /// Removes a quest from the diary.
        /// </summary>
        /// <param name="quest">Quest to remove.</param>
        void Remove(IQuest quest);

        /// <summary>
        /// Updates the quest diary with an action type.
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="values"></param>
        void Update(QuestActionType actionType, params object[] values);
    }
}
