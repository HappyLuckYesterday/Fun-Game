using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using System.Diagnostics;

namespace Rhisis.Game
{
    [DebuggerDisplay("{Name} ({Status})")]
    public class MessengerContact : IContact
    {
        public int Id { get; }

        public int Channel { get; }

        public bool IsBlocked { get; set; }

        public string Name { get; }

        public MessengerStatusType Status { get; set; } = MessengerStatusType.Online;

        public DefineJob.Job Job { get; set; }

        public MessengerContact(IPlayer player, int channel)
        {
            Id = player.CharacterId;
            Channel = channel;
            Name = player.Name;
            Job = player.Job.Id;
        }

        public MessengerContact(int playerId, int channel, string name, DefineJob.Job job, MessengerStatusType messengerStatus, bool isBlocked)
        {
            Id = playerId;
            Channel = channel;
            Name = name;
            Job = job;
            Status = messengerStatus;
            IsBlocked = isBlocked;
        }
    }
}