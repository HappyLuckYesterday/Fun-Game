using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Abstractions.Entities
{
    /// <summary>
    /// Describes the NPC entity.
    /// </summary>
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
        /// Gets a value that indicates if the NPC can buff.
        /// </summary>
        bool CanBuff => Data?.CanBuff ?? false;

        /// <summary>
        /// Gets the NPC quests.
        /// </summary>
        IEnumerable<IQuestScript> Quests { get; }

        /// <summary>
        /// Gets a value that indicates if the NPC has quests.
        /// </summary>
        bool HasQuests => Quests.Any();

        /// <summary>
        /// Gets the NPC chat feature.
        /// </summary>
        IChat Chat { get; }
    }
}
