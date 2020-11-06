using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common.Resources.Quests;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechansim to manage player quests.
    /// </summary>
    public interface IQuestSystem
    {
        /// <summary>
        /// Check if the player can start the given quest script.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Quest script to start.</param>
        /// <returns>True if the player can start the quest; false otherwise.</returns>
        bool CanStartQuest(IPlayer player, INpc npc, IQuestScript quest);

        /// <summary>
        /// Check if the player can finish quests from the given npc.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Quest sript to finish.</param>
        /// <returns>True if the player can finish a quest; false otherwise.</returns>
        bool CanFinishQuest(IPlayer player, INpc npc, IQuestScript quest);

        /// <summary>
        /// Process a quest script state.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current NPC holding the quest.</param>
        /// <param name="quest">Quest script.</param>
        /// <param name="state">Quest script state.</param>
        void ProcessQuest(IPlayer player, INpc npc, IQuestScript quest, QuestStateType state);

        /// <summary>
        /// Sends the quest dialogs for available and in progress quests.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holing quests.</param>
        void SendQuestsInfo(IPlayer player, INpc npc);
    }
}
