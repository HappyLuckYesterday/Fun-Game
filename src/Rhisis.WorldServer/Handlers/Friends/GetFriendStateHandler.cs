using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class GetFriendStateHandler
    {
        private readonly IPlayerCache _playerCache;

        public GetFriendStateHandler(IPlayerCache playerCache)
        {
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.GETFRIENDSTATE)]
        public void OnExecute(IPlayer player, GetFriendStatePacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"Player ids doesn't match.");
            }

            IEnumerable<IContact> friends = player.Messenger.Friends.Where(x => !x.IsBlocked).Select(x => GetFriend(player, x));
            IEnumerable<IContact> blockedFriends = player.Messenger.Friends.Where(x => x.IsBlocked).Select(x => GetFriend(player, x));

            using var friendStatePacket = new Rhisis.Game.Protocol.Packets.Friends.GetFriendStatePacket(friends, blockedFriends);
            player.Send(friendStatePacket);
        }

        private IContact GetFriend(IPlayer player, IContact friendContact)
        {
            IContact friend = friendContact.Clone();
            CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(friend.Id);

            if (cachedPlayer != null)
            {
                CachedPlayerFriend playerFriend = cachedPlayer.Friends.FirstOrDefault(x => x.FriendId == player.CharacterId);

                if (playerFriend != null)
                {
                    friend.Status = playerFriend.IsBlocked ? MessengerStatusType.Offline : cachedPlayer.MessengerStatus;
                }
            }

            return friend;
        }
    }
}
