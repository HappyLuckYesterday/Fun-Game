using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Game.Protocol.Packets.Friends;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class RemoveFriendHandler
    {
        private readonly IMessaging _messaging;

        public RemoveFriendHandler(IMessaging messaging)
        {
            _messaging = messaging;
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
    }
}
