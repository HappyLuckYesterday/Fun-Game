using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface INpc : IWorldObject, IInteligentEntity
    {
        /// <summary>
        /// Gets the NPC key.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the NPC mover data.
        /// </summary>
        NpcData Data { get; }

        /// <summary>
        /// Gets the npc shop item containers.
        /// </summary>
        /// <remarks>
        /// One item container represents one shop tab.
        /// </remarks>
        IItemContainer[] Shop { get; }

        /// <summary>
        /// Gets a value that indicates if the NPC has a shop.
        /// </summary>
        bool HasShop => Shop != null && Shop.Any();

        /// <summary>
        /// Gets the NPC Dialog.
        /// </summary>
        DialogData Dialog { get; }

        /// <summary>
        /// Gets a value that indicates if the NPC has a dialog.
        /// </summary>
        bool HasDialog => Dialog != null;

        /// <summary>
        /// Gets the NPC quests.
        /// </summary>
        IEnumerable<IQuestScript> Quests { get; }

        /// <summary>
        /// Gets a value that indicates if the NPC has quests.
        /// </summary>
        bool HasQuests => Quests.Any();

        void OpenDialog(IPlayer player, string dialogKey, int questId);

        void Speak(string text);
    }
}
