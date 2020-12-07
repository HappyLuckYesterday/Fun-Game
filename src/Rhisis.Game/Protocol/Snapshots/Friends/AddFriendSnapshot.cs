using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Friends
{
    public class AddFriendSnapshot : FFSnapshot
    {
        public AddFriendSnapshot(IPlayer sender, IPlayer target)
            : base(SnapshotType.ADDFRIEND, sender.Id)
        {
            WriteUInt32(target.Id);
            WriteString(target.Name);
        }
    }
}
