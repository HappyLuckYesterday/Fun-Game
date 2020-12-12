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

        public PlayerMessengerStatusUpdateMessage(IWorldServer worldServer)
        {
            _worldServer = worldServer;
        }

        [HandlerAction(typeof(PlayerMessengerStatusUpdate))]
        public void OnPlayerStatusUpdateMessage(PlayerMessengerStatusUpdate playerMessengerStatusUpdate)
        {
            int playerConnectedId = playerMessengerStatusUpdate.Id;
            IEnumerable<IPlayer> players = _worldServer.ConnectedPlayers
                .Where(x => x.Spawned &&
                            x.CharacterId != playerConnectedId &&
                            x.Messenger.Friends.Contains((uint)playerConnectedId));

            foreach (IPlayer player in players)
            {
                player.Messenger.OnFriendStatusChanged(playerConnectedId, playerMessengerStatusUpdate.Status);
            }
        }
    }
}
