using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Common;
using System;
using System.Linq;

namespace Rhisis.Game.Caching
{
    [Injectable]
    internal sealed class PlayerCache : IPlayerCache
    {
        private readonly IRhisisCache _playerCache;
        private readonly IRhisisDatabase _database;

        public PlayerCache(IRhisisCacheManager cacheManager, IRhisisDatabase database)
        {
            _playerCache = cacheManager.GetCache(CacheType.ClusterPlayers);
            _database = database;
        }

        public CachedPlayer GetCachedPlayer(int playerId)
        {
            return _playerCache.Get<CachedPlayer>(playerId.ToString()) ?? LoadCachedPlayer(playerId);
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
            _playerCache.Set(player.Id.ToString(), player);
        }
    }
}
