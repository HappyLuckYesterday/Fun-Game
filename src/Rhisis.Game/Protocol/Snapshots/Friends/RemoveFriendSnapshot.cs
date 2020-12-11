using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Friends
{
    public class RemoveFriendSnapshot : FFSnapshot
    {
        public RemoveFriendSnapshot(IPlayer player, int removedFriendId)
            : base(SnapshotType.REMOVEFRIEND, player.Id)
        {
            WriteUInt32((uint)removedFriendId);
        }
    }
}
