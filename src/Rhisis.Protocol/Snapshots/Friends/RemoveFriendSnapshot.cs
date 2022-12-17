using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Friends;

public class RemoveFriendSnapshot : FFSnapshot
{
    public RemoveFriendSnapshot(IPlayer player, int removedFriendId)
        : base(SnapshotType.REMOVEFRIEND, player.Id)
    {
        WriteUInt32((uint)removedFriendId);
    }
}
