using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Caching;

namespace Rhisis.Game.Caching
{
    [Injectable]
    internal sealed class PlayerCache : IPlayerCache
    {
        private readonly IRhisisCache _playerCache;

        public PlayerCache(IRhisisCacheManager cacheManager)
        {
            _playerCache = cacheManager.GetCache(CacheType.ClusterPlayers);
        }

        public CachedPlayer GetCachedPlayer(int playerId)
        {
            return _playerCache.Get<CachedPlayer>(playerId.ToString());
        }

        public void SetCachedPlayer(CachedPlayer player)
        {
            _playerCache.Set(player.Id.ToString(), player);
        }
    }
}
