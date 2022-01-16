using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Friends
{
    public class AddFriendSnapshot : FFSnapshot
    {
        public AddFriendSnapshot(IPlayer sender, IPlayer target)
            : base(SnapshotType.ADDFRIEND, sender.Id)
        {
            WriteUInt32((uint)target.CharacterId);
            WriteString(target.Name);
        }
    }
}
