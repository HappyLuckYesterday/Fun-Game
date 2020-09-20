using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IDialogSystem
    {
        /// <summary>
        /// Opens a NPC dialog at a given dialog key.
        /// </summary>
        /// <param name="npc">Current NPC.</param>
        /// <param name="player">Target player.</param>
        /// <param name="dialogKey">Dialog key.</param>
        /// <param name="questId">Quest id.</param>
        void OpenNpcDialog(INpc npc, IPlayer player, string dialogKey, int questId = 0);
    }
}
