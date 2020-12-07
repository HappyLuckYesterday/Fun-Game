using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class AddFriendRequestHandler
    {
        private readonly IWorldServer _worldServer;

        public AddFriendRequestHandler(IWorldServer worldServer)
        {
            _worldServer = worldServer;
        }

        [HandlerAction(PacketType.ADDFRIENDREQEST)]
        public void OnExecute(IPlayer player, AddFriendRequestPacket packet)
        {
            if (player.CharacterId != packet.CurrentPlayerId)
            {
                throw new InvalidOperationException($"The player ids doesn't match.");
            }

            IPlayer targetPlayer = _worldServer.GetPlayerEntityByCharacterId(packet.TargetPlayerId);

            if (targetPlayer is null)
            {
                throw new InvalidOperationException($"Cannot find player with id: {packet.TargetPlayerId}.");
            }

            if (!player.Messenger.Friends.Contains(targetPlayer.Id))
            {
                if (!targetPlayer.Battle.IsFighting)
                {
                    player.Messenger.SendFriendRequest(targetPlayer);
                }
            }
        }
    }
}
