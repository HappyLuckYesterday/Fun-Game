using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Entities;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class FriendInterceptStateHandler
    {
        private readonly IPlayerCache _playerCache;

        public FriendInterceptStateHandler(IPlayerCache playerCache)
        {
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.FRIENDINTERCEPTSTATE)]
        public void OnExecute(IPlayer player, FriendInterceptStatePacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"The player ids doesn't match.");
            }

            player.Messenger.ToggleFriendBlockState((int)packet.FriendPlayerId);

            CachedPlayer cachedPlayer = _playerCache.Get(player.CharacterId);
            CachedPlayerFriend cachedFriend = cachedPlayer.Friends.FirstOrDefault();

            if (cachedPlayer != null)
            {
                cachedFriend.IsBlocked = player.Messenger.Friends.Get((int)packet.FriendPlayerId).IsBlocked;

                _playerCache.Set(cachedPlayer);
            }

            //_messaging.Publish(new PlayerMessengerBlockFriend(player.CharacterId, (int)packet.FriendPlayerId));
        }
    }
}
