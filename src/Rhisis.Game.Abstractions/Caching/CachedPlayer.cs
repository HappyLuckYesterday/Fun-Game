using Rhisis.Game.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.Game.Abstractions.Caching
{
    [DebuggerDisplay("{Name}({Id}) Lv.{Level} (Channel: {Channel})")]
    public class CachedPlayer
    {
        public int Id { get; set; }

        public int Channel { get; set; }

        public string Name { get; set; }

        public GenderType Gender { get; set; }

        public int Level { get; set; }

        public int Version { get; set; } = 0;

        public DefineJob.Job Job { get; set; }

        public bool IsOnline { get; set; }

        public MessengerStatusType MessengerStatus { get; set; } = MessengerStatusType.Offline;

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