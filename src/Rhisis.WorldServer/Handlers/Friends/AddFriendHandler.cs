using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.World.Friends;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class AddFriendHandler
    {
        private readonly IWorldServer _worldServer;
        private readonly IPlayerCache _playerCache;

        public AddFriendHandler(IWorldServer worldServer, IPlayerCache playerCache)
        {
            _worldServer = worldServer;
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.ADDFRIEND)]
        public void OnExecute(IPlayer player, AddFriendPacket packet)
        {
            if (player.CharacterId != packet.FriendId || player.Job.Id != packet.FriendJob || player.Appearence.Gender != packet.FriendGender)
            {
                throw new InvalidOperationException($"Invalid friend data.");
            }

            IPlayer sender = _worldServer.GetPlayerEntity(packet.SenderId);

            if (sender is null)
            {
                throw new InvalidOperationException($"Cannot find player with object id: {packet.SenderId}.");
            }

            if (sender.Appearence.Gender != packet.SenderGender || sender.Job.Id != packet.FriendJob)
            {
                throw new InvalidOperationException($"Invalid sender data.");
            }

            if (!sender.Messenger.Friends.IsFull && !player.Messenger.Friends.IsFull)
            {
                sender.Messenger.AddFriend(player);
                player.Messenger.AddFriend(sender);

                CachedPlayer senderCachedPlayer = _playerCache.Get(sender.CharacterId);
                CachedPlayer friendCachedPlayer = _playerCache.Get(player.CharacterId);

                senderCachedPlayer.Friends.Add(new CachedPlayerFriend(player.CharacterId));
                friendCachedPlayer.Friends.Add(new CachedPlayerFriend(sender.CharacterId));

                _playerCache.Set(senderCachedPlayer);
                _playerCache.Set(friendCachedPlayer);
            }
            else
            {
                // TODO: send messenger full message.
            }
        }
    }
}
