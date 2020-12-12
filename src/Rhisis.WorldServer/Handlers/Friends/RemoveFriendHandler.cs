using Rhisis.Database;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Game.Protocol.Packets.Friends;
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
        private readonly IRhisisDatabase _database;
        private readonly IWorldServer _worldServer;

        public RemoveFriendHandler(IMessaging messaging, IRhisisDatabase database, IWorldServer worldServer)
        {
            _messaging = messaging;
            _database = database;
            _worldServer = worldServer;
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
