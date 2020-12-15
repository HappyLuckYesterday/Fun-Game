using Rhisis.Game.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.Game.Abstractions.Caching
{
    [DebuggerDisplay("{Name}({Id}) Lv.{Level} (Channel: {Channel})")]
    public class CachedPlayer
    {
        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player channel.
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// Gets or sets the player name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the player gender.
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// Gets or sets the player level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the player version.
        /// </summary>
        public int Version { get; set; } = 0;

        /// <summary>
        /// Gets or sets the player job.
        /// </summary>
        public DefineJob.Job Job { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the player is online.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Gets or sets the player messenger status.
        /// </summary>
        public MessengerStatusType MessengerStatus { get; set; } = MessengerStatusType.Offline;

        /// <summary>
        /// Gets or sets the player friend list.
        /// </summary>
        public List<CachedPlayerFriend> Friends { get; set; } = new List<CachedPlayerFriend>();

        public CachedPlayer()
        {
        }

        public CachedPlayer(int id, int channel, string name, GenderType gender)
        {
            Id = id;
            Channel = channel;
            Name = name;
            Gender = gender;
        }
    }
}