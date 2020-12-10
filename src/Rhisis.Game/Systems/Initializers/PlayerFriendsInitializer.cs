using Microsoft.EntityFrameworkCore;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems.Initializers
{
    [Injectable]
    public sealed class PlayerFriendsInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;
        private readonly IPlayerCache _playerCache;

        public PlayerFriendsInitializer(IRhisisDatabase database, IPlayerCache playerCache)
        {
            _database = database;
            _playerCache = playerCache;
        }

        public void Load(IPlayer player)
        {
            IEnumerable<DbFriend> friends = _database.Friends
                .Include(x => x.Friend)
                .Where(x => x.CharacterId == player.CharacterId)
                .AsNoTracking()
                .AsEnumerable();

            foreach (DbFriend friend in friends)
            {
                CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(friend.FriendId);

                if (cachedPlayer is null)
                {
                    cachedPlayer = _playerCache.LoadCachedPlayer(friend.FriendId);
                }

                var contact = new MessengerContact(cachedPlayer.Id, 
                    cachedPlayer.Channel, 
                    cachedPlayer.Name, 
                    cachedPlayer.Job, 
                    cachedPlayer.MessengerStatus, 
                    friend.IsBlocked);

                player.Messenger.Friends.Add(contact);
            }

            player.Messenger.Status = MessengerStatusType.Online;
        }

        public void Save(IPlayer player)
        {
            DbCharacter character = _database.Characters
                .Include(x => x.Friends)
                .FirstOrDefault(x => x.Id == player.CharacterId);

            if (character is null)
            {
                throw new InvalidOperationException($"Failed to find player with id: {player.CharacterId} in database.");
            }

            character.Friends.Clear();

            foreach (IContact contact in player.Messenger.Friends)
            {
                character.Friends.Add(new DbFriend
                {
                    FriendId = (int)contact.Id,
                    IsBlocked = contact.IsBlocked
                });
            }

            _database.SaveChanges();
        }
    }
}
