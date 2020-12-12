using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class FriendInterceptStateHandler
    {
        private readonly IMessaging _messaging;

        public FriendInterceptStateHandler(IMessaging messaging)
        {
            _messaging = messaging;
        }

        [HandlerAction(PacketType.FRIENDINTERCEPTSTATE)]
        public void OnExecute(IPlayer player, FriendInterceptStatePacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"The player ids doesn't match.");
            }

            player.Messenger.SetFriendBlockState((int)packet.FriendPlayerId);
        }
    }
}
