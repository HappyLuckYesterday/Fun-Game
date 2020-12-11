using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets;
using Rhisis.Game.Protocol.Packets.Friends;
using Rhisis.Game.Protocol.Snapshots.Friends;
using Rhisis.Network.Snapshots;
using Sylver.Network.Data;

namespace Rhisis.Game.Abstractions.Components
{
    public class Messenger : GameFeature, IMessenger
    {
        private readonly IPlayer _player;
        private readonly int _currentChannelId;
        private readonly IPlayerCache _playerCache;

        public MessengerStatusType Status { get; set; }

        public IContactList Friends { get; }

        public Messenger(IPlayer player, int currentChannelId, int maximumFriendCount)
        {
            _player = player;
            _currentChannelId = currentChannelId;
            Friends = new MessengerContactList(maximumFriendCount);
        }

        public void SendFriendRequest(IPlayer targetPlayer)
        {
            using var snapshot = new AddFriendRequestSnapshot(_player, targetPlayer);

            targetPlayer.Send(snapshot);
        }

        public void AddFriend(IPlayer playerToAdd)
        {
            var contact = new MessengerContact(playerToAdd, _currentChannelId);

            Friends.Add(contact);

            using var snapshot = new AddFriendSnapshot(_player, playerToAdd);
            snapshot.Merge(new DefinedTextSnapshot(_player, DefineText.TID_GAME_MSGINVATECOM, playerToAdd.Name));

            _player.Send(snapshot);
        }

        public void OnFriendConnected(int friendPlayerId, MessengerStatusType statusType)
        {
            IContact friend = Friends.Get(friendPlayerId);

            if (friend is null)
            {
                return;
            }

            friend.Status = statusType;

            using var friendJoinGame = new AddFriendJoinPacket(friend);
            _player.Send(friendJoinGame);
        }

        public void OnFriendStatusChanged(int friendPlayerId, MessengerStatusType statusType)
        {
            IContact friend = Friends.Get(friendPlayerId);

            if (friend is null)
            {
                return;
            }

            MessengerStatusType oldStatus = friend.Status;
            friend.Status = statusType;

            if (oldStatus == MessengerStatusType.Offline)
            {
                using var friendJoinGame = new AddFriendJoinPacket(friend);
                _player.Send(friendJoinGame);
            }
            else
            {
                using var setStatePacket = new SetFriendStatePacket(friend.Id, friend.Status);
                _player.Send(setStatePacket);
            }
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.WriteInt32((int)Status);
            Friends.Serialize(packet);
        }
    }
}
