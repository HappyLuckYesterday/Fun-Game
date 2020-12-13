using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Messages;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends.Messages
{
    [Handler]
    public class PlayerMessengerBlockFriendMessage
    {
        private readonly IWorldServer _server;
        private readonly IPlayerCache _playerCache;

        public PlayerMessengerBlockFriendMessage(IWorldServer server, IPlayerCache playerCache)
        {
            _server = server;
            _playerCache = playerCache;
        }

        [HandlerAction(typeof(PlayerMessengerBlockFriend))]
        public void OnPlayerMessengerBlockFriend(PlayerMessengerBlockFriend blockedFriendMessage)
        {
            IPlayer blockedFriendPlayer = _server.GetPlayerEntityByCharacterId((uint)blockedFriendMessage.FriendId);

            if (blockedFriendPlayer is null)
            {
                return;
            }

            CachedPlayer playerBlockingFriend = _playerCache.GetCachedPlayer(blockedFriendMessage.PlayerId);
            CachedPlayerFriend blockedFriend = playerBlockingFriend.Friends.FirstOrDefault(x => x.FriendId == blockedFriendMessage.FriendId);

            if (blockedFriend is null)
            {
                return;
            }

            if (blockedFriend.IsBlocked)
            {
                blockedFriendPlayer.Messenger.OnFriendStatusChanged(blockedFriendMessage.PlayerId, MessengerStatusType.Offline);
            }
            else
            {
                blockedFriendPlayer.Messenger.OnFriendStatusChanged(blockedFriendMessage.PlayerId, playerBlockingFriend.MessengerStatus);
            }
        }
    }
}
