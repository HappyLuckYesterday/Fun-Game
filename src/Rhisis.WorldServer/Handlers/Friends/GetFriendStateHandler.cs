using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class GetFriendStateHandler
    {
        [HandlerAction(PacketType.GETFRIENDSTATE)]
        public void OnExecute(IPlayer player, GetFriendStatePacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"Player ids doesn't match.");
            }

            IEnumerable<IContact> friends = player.Messenger.Friends.Where(x => !x.IsBlocked);
            IEnumerable<IContact> blockedFriends = player.Messenger.Friends.Where(x => x.IsBlocked);

            using var friendStatePacket = new Rhisis.Game.Protocol.Packets.Friends.GetFriendStatePacket(friends, blockedFriends);
            player.Send(friendStatePacket);
        }
    }
}
