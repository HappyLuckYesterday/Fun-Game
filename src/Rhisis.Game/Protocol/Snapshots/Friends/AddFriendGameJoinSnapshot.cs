using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Friends
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
