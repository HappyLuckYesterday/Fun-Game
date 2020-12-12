using Rhisis.Database;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Protocol.Messages;
using Sylver.HandlerInvoker.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends.Messages
{
    [Handler]
    public class PlayerMessengerRemoveFriendMessage
    {
        private readonly IWorldServer _worldServer;
        private readonly IRhisisDatabase _database;

        public PlayerMessengerRemoveFriendMessage(IWorldServer worldServer, IRhisisDatabase database)
        {
            _worldServer = worldServer;
            _database = database;
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
        }
    }
}
