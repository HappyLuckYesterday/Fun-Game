using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Protocol.Messages;
using Sylver.HandlerInvoker.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends.Messages
{
    [Handler]
    public class PlayerMessengerStatusUpdateMessage
    {
        private readonly IWorldServer _worldServer;
        private readonly IPlayerCache _playerCache;

        public PlayerMessengerStatusUpdateMessage(IWorldServer worldServer, IPlayerCache playerCache)
        {
            _worldServer = worldServer;
            _playerCache = playerCache;
        }

        [HandlerAction(typeof(PlayerMessengerStatusUpdate))]
        public void OnPlayerStatusUpdateMessage(PlayerMessengerStatusUpdate playerMessengerStatusUpdate)
        {
            int playerIdUpdatingStatus = playerMessengerStatusUpdate.Id;
            CachedPlayer cachedPlayerUpdatingStatus = _playerCache.GetCachedPlayer(playerIdUpdatingStatus);
            IEnumerable<IPlayer> players = _worldServer.ConnectedPlayers
                .Where(x => x.Spawned &&
                            x.CharacterId != playerIdUpdatingStatus &&
                            x.Messenger.Friends.Contains((uint)playerIdUpdatingStatus));

            foreach (IPlayer player in players)
            {
                CachedPlayerFriend playerFriend = cachedPlayerUpdatingStatus.Friends.FirstOrDefault(x => x.FriendId == player.CharacterId);

                if (playerFriend is null)
                {
                    continue;
                }

                if (!playerFriend.IsBlocked)
                {
                    player.Messenger.OnFriendStatusChanged(playerIdUpdatingStatus, playerMessengerStatusUpdate.Status);
                }
            }
        }
    }
}
