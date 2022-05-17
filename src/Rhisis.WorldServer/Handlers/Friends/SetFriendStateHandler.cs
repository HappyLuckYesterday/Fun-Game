using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Protocol;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class SetFriendStateHandler
    {
        private readonly IPlayerCache _playerCache;

        public SetFriendStateHandler(IPlayerCache playerCache)
        {
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.SETFRIENDSTATE)]
        public void OnExecute(IPlayer player, Protocol.Packets.Client.World.Friends.SetFriendStatePacket packet)
        {
            if (player.CharacterId == packet.PlayerId)
            {
                if (packet.Status < MessengerStatusType.Online || packet.Status > MessengerStatusType.FriendStat)
                {
                    throw new InvalidOperationException($"Invalid messenger status.");
                }

                player.Messenger.Status = packet.Status;

                CachedPlayer cachedPlayer = _playerCache.Get(player.CharacterId);

                if (cachedPlayer != null)
                {
                    cachedPlayer.MessengerStatus = player.Messenger.Status;
                    cachedPlayer.Version++;
                    _playerCache.Set(cachedPlayer);
                }

                using var setStatusStatePacket = new Protocol.Packets.Server.World.Friends.SetFriendStatePacket(player.CharacterId, player.Messenger.Status);
                player.Send(setStatusStatePacket);

                //_messaging.Publish(new PlayerMessengerStatusUpdate
                //{
                //    Id = player.CharacterId,
                //    Status = player.Messenger.Status
                //});
            }
        }
    }
}
