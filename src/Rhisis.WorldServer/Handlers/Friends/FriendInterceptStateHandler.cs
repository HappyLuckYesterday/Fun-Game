using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class FriendInterceptStateHandler
    {
        private readonly IMessaging _messaging;
        private readonly IPlayerCache _playerCache;

        public FriendInterceptStateHandler(IMessaging messaging, IPlayerCache playerCache)
        {
            _messaging = messaging;
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.FRIENDINTERCEPTSTATE)]
        public void OnExecute(IPlayer player, FriendInterceptStatePacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"The player ids doesn't match.");
            }

            player.Messenger.SetFriendBlockState((int)packet.FriendPlayerId);

            CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(player.CharacterId);
            CachedPlayerFriend cachedFriend = cachedPlayer.Friends.FirstOrDefault();

            if (cachedPlayer != null)
            {
                cachedFriend.IsBlocked = player.Messenger.Friends.Get((int)packet.FriendPlayerId).IsBlocked;

                _playerCache.SetCachedPlayer(cachedPlayer);
            }

            _messaging.Publish(new PlayerMessengerBlockFriend(player.CharacterId, (int)packet.FriendPlayerId));
        }
    }
}
