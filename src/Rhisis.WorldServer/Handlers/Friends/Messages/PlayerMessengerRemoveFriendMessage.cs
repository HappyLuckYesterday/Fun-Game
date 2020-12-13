using Rhisis.Database;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Protocol.Messages;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends.Messages
{
    [Handler]
    public class PlayerMessengerRemoveFriendMessage
    {
        private readonly IWorldServer _worldServer;
        private readonly IRhisisDatabase _database;
        private readonly IPlayerCache _playerCache;

        public PlayerMessengerRemoveFriendMessage(IWorldServer worldServer, IRhisisDatabase database, IPlayerCache playerCache)
        {
            _worldServer = worldServer;
            _database = database;
            _playerCache = playerCache;
        }

        [HandlerAction(typeof(PlayerMessengerRemoveFriend))]
        public void OnPlayerMessengerRemoveFriendMessage(PlayerMessengerRemoveFriend friendRemovalMessage)
        {
            int playerId = friendRemovalMessage.PlayerId;
            IPlayer removedPlayer = _worldServer.ConnectedPlayers
                .Where(x => x.Spawned &&
                            x.CharacterId == friendRemovalMessage.RemovedFriendId &&
                            x.Messenger.Friends.Contains((uint)playerId))
                .FirstOrDefault();

            if (removedPlayer != null)
            {
                removedPlayer.Messenger.RemoveFriend(playerId);
            }

            var friend = _database.Friends.FirstOrDefault(x => x.CharacterId == friendRemovalMessage.RemovedFriendId && x.FriendId == playerId);

            if (friend != null)
            {
                _database.Friends.Remove(friend);
                _database.SaveChangesAsync();
            }

            CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(removedPlayer.CharacterId);
            CachedPlayerFriend playerFriend = cachedPlayer.Friends.FirstOrDefault(x => x.FriendId == playerId);

            if (playerFriend != null)
            {
                cachedPlayer.Friends.Remove(playerFriend);
                _playerCache.SetCachedPlayer(cachedPlayer);
            }
        }
    }
}
