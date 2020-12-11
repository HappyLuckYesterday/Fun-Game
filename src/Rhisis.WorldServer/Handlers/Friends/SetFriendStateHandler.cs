using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Friends;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class SetFriendStateHandler
    {
        private readonly IMessaging _messaging;
        private readonly IPlayerCache _playerCache;

        public SetFriendStateHandler(IMessaging messaging, IPlayerCache playerCache)
        {
            _messaging = messaging;
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.SETFRIENDSTATE)]
        public void OnExecute(IPlayer player, SetFriendStatePacket packet)
        {
            if (player.CharacterId == packet.PlayerId)
            {
                if (packet.Status < MessengerStatusType.Online || packet.Status > MessengerStatusType.FriendStat)
                {
                    throw new InvalidOperationException($"Invalid messenger status.");
                }

                player.Messenger.Status = packet.Status;

                CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(player.CharacterId);

                if (cachedPlayer != null)
                {
                    cachedPlayer.MessengerStatus = player.Messenger.Status;
                    _playerCache.SetCachedPlayer(cachedPlayer);
                }

                using var setStatusStatePacket = new Rhisis.Game.Protocol.Packets.Friends.SetFriendStatePacket(player.CharacterId, player.Messenger.Status);
                player.Send(setStatusStatePacket);

                _messaging.Publish(new PlayerMessengerStatusUpdate
                {
                    Id = player.CharacterId,
                    Status = player.Messenger.Status
                });
            }
        }
    }
}
