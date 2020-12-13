using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Friends
{
    public class AddFriendRequestSnapshot : FFSnapshot
    {
        public AddFriendRequestSnapshot(IPlayer requestSender, IPlayer target)
            : base(SnapshotType.ADDFRIENDREQEST, target.Id)
        {
            WriteUInt32(requestSender.Id);
            WriteByte((byte)requestSender.Appearence.Gender);
            WriteInt32((int)requestSender.Job.Id);
            WriteString(requestSender.Name);
        }
    }
}
