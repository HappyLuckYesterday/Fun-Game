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
    public class RemoveFriendHandler
    {
        private readonly IMessaging _messaging;
        private readonly IPlayerCache _playerCache;

        public RemoveFriendHandler(IMessaging messaging, IPlayerCache playerCache)
        {
            _messaging = messaging;
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
            _messaging.Publish(new PlayerMessengerRemoveFriend(player.CharacterId, packet.FriendId));

            CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(player.CharacterId);
            CachedPlayerFriend playerFriend = cachedPlayer.Friends.FirstOrDefault(x => x.FriendId == packet.FriendId);

            if (cachedPlayer != null)
            {
                cachedPlayer.Friends.Remove(playerFriend);
                _playerCache.SetCachedPlayer(cachedPlayer);
            }
        }
    }
}
