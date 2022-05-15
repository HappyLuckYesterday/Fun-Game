using Rhisis.Abstractions.Caching;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Common;
using Rhisis.Infrastructure.Caching;
using Rhisis.Infrastructure.Persistance;
using System;
using System.Linq;

namespace Rhisis.Game.Caching
{
    [Injectable]
    internal sealed class PlayerCache : IPlayerCache
    {
        private readonly RhisisCache<int, CachedPlayer> _playerCache = new();
        private readonly IRhisisDatabase _database;

        public PlayerCache(IRhisisDatabase database)
        {
            _database = database;
        }

        public CachedPlayer Get(int playerId)
        {
            return _playerCache.Get(playerId) ?? Load(playerId);
        }

        public CachedPlayer Get(string playerName)
        {
            int playerId = _database.Characters
                .Where(x => x.Name.ToLower() == playerName.ToLower())
                .Select(x => x.Id)
                .FirstOrDefault();

            return Get(playerId);
        }

        public CachedPlayer Load(int playerId)
        {
            var cachedPlayer = _database.Characters.Where(x => x.Id == playerId)
                .Select(x => new CachedPlayer(x.Id, default, x.Name, (GenderType)x.Gender)
                {
                    Version = default,
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

            Set(cachedPlayer);

            return cachedPlayer;
        }

        public void Set(CachedPlayer player)
        {
            player.Version++;

            _playerCache.Set(player.Id, player);
        }
    }
}
