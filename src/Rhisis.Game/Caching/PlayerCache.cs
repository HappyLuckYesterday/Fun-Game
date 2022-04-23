using Rhisis.Abstractions.Caching;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Common;
using Rhisis.Infrastructure.Persistance;
using System;
using System.Linq;

namespace Rhisis.Game.Caching
{
    [Injectable]
    internal sealed class PlayerCache : IPlayerCache
    {
        private readonly IRhisisCache<CachedPlayer> _playerCache;
        private readonly IRhisisDatabase _database;

        public PlayerCache(IRhisisCache<CachedPlayer> playerCache, IRhisisDatabase database)
        {
            _playerCache = playerCache;
            _database = database;
        }

        public CachedPlayer GetCachedPlayer(int playerId)
        {
            return _playerCache.Get(playerId) ?? LoadCachedPlayer(playerId);
        }

        public CachedPlayer GetCachedPlayer(string playerName)
        {
            int playerId = _database.Characters
                .Where(x => x.Name.ToLower() == playerName.ToLower())
                .Select(x => x.Id)
                .FirstOrDefault();

            return GetCachedPlayer(playerId);
        }

        public CachedPlayer LoadCachedPlayer(int playerId)
        {
            var cachedPlayer = _database.Characters.Where(x => x.Id == playerId)
                .Select(x => new CachedPlayer(x.Id, default, x.Name, (GenderType)x.Gender)
                {
                    Version = 1,
                    Level = x.Level,
                    Job = (DefineJob.Job)x.JobId,
                    IsOnline = false,
                    MessengerStatus = MessengerStatusType.Offline
                })
                .FirstOrDefault();

            if (cachedPlayer is null)
            {
                throw new InvalidOperationException($"Failed to fetch player with id: {playerId} from database.");
            }

            SetCachedPlayer(cachedPlayer);

            return cachedPlayer;
        }

        public void SetCachedPlayer(CachedPlayer player)
        {
            _playerCache.Set(player.Id, player);
        }
    }
}
