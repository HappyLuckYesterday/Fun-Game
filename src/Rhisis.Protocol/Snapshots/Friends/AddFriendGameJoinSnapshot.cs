using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Friends
{
    public class AddFriendGameJoinSnapshot : FFSnapshot
    {
        public AddFriendGameJoinSnapshot(IPlayer player)
            : base(SnapshotType.ADDFRIENDGAMEJOIN, player.Id)
        {
            player.Messenger.Serialize(this);
        }
    }
}
