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
    public class RemoveFriendHandler
    {
        private readonly IPlayerCache _playerCache;

        public RemoveFriendHandler(IPlayerCache playerCache)
        {
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.REMOVEFRIEND)]
        public void OnExecute(IPlayer player, RemoveFriendPacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"Player ids doens't match.");
            }

            player.Messenger.RemoveFriend(packet.FriendId);
            //_messaging.Publish(new PlayerMessengerRemoveFriend(player.CharacterId, packet.FriendId));

            CachedPlayer cachedPlayer = _playerCache.Get(player.CharacterId);
            CachedPlayerFriend playerFriend = cachedPlayer.Friends.FirstOrDefault(x => x.FriendId == packet.FriendId);

            if (cachedPlayer != null)
            {
                cachedPlayer.Friends.Remove(playerFriend);
                _playerCache.Set(cachedPlayer);
            }
        }
    }
}
