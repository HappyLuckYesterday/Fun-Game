using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using System;

namespace Rhisis.Game.Abstractions.Entities
{
    /// <summary>
    /// Describes the player entity.
    /// </summary>
    public interface IPlayer : IHuman
    {
        /// <summary>
        /// Gets the player game connection.
        /// </summary>
        IGameConnection Connection { get; }

        /// <summary>
        /// Gets the player login date.
        /// </summary>
        DateTime LoggedInAt { get; }

        /// <summary>
        /// Gets the player character id.
        /// </summary>
        /// <remarks>
        /// This is the database id.
        /// </remarks>
        int CharacterId { get; }

        /// <summary>
        /// Gets the player experience.
        /// </summary>
        IExperience Experience { get; }

        /// <summary>
        /// Gets the player gold.
        /// </summary>
        IGold Gold { get; }

        /// <summary>
        /// Gets the player slot.
        /// </summary>
        int Slot { get; }

        /// <summary>
        /// Gets or sets the player death level.
        /// </summary>
        int DeathLevel { get; set; }

        /// <summary>
        /// Gets the player authority.
        /// </summary>
        AuthorityType Authority { get; }

        /// <summary>
        /// Gets or sets the player mode.
        /// </summary>
        ModeType Mode { get; set; }

        /// <summary>
        /// Gets or sets the player job.
        /// </summary>
        JobData Job { get; set; }

        /// <summary>
        /// Gets the player statistics.
        /// </summary>
        new IPlayerStatistics Statistics { get; }

        /// <summary>
        /// Gets the player inventory.
        /// </summary>
        IInventory Inventory { get; }

        /// <summary>
        /// Gets the player chat.
        /// </summary>
        IChat Chat { get; }

        /// <summary>
        /// Gets the player battle context.
        /// </summary>
        IBattle Battle { get; }

        /// <summary>
        /// Gets the player quest diary.
        /// </summary>
        IQuestDiary Quests { get; }

        /// <summary>
        /// Gets the player skill tree.
        /// </summary>
        ISkillTree SkillTree { get; }

        /// <summary>
        /// Gets the player taskbar.
        /// </summary>
        ITaskbar Taskbar { get; }

        /// <summary>
        /// Gets the player messenger.
        /// </summary>
        IMessenger Messenger { get; }

        /// <summary>
        /// Gets or sets the current shop name the player is visiting.
        /// </summary>
        string CurrentNpcShopName { get; set; }

        /// <summary>
        /// Teleports the current player to the given position.
        /// </summary>
        /// <param name="position">Destination position.</param>
        /// <param name="sendToPlayer"></param>
        void Teleport(Vector3 position, bool sendToPlayer = true);

        /// <summary>
        /// Teleports the current player to the given position on the given map id.
        /// </summary>
        /// <param name="position">Destination position.</param>
        /// <param name="mapId">Destination map id.</param>
        /// <param name="sendToPlayer"></param>
        void Teleport(Vector3 position, int mapId, bool sendToPlayer = true);

        /// <summary>
        /// Change the current player job.
        /// </summary>
        /// <param name="newJob">New job.</param>
        void ChangeJob(DefineJob.Job newJob);

        /// <summary>
        /// Updates the player cache information.
        /// </summary>
        void UpdateCache();
    }
}
