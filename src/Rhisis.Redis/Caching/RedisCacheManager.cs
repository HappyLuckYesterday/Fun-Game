using Rhisis.Game.Abstractions.Caching;
using Rhisis.Redis.Internal;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Redis.Caching
{
    internal class RedisCacheManager : IRhisisCacheManager
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly ConcurrentDictionary<int, IRhisisCache> _caches;
        private readonly RedisConfiguration _redisConfiguration;

        public RedisCacheManager(RedisConnection redisConnection, RedisConfiguration redisConfiguration)
        {
            _connection = redisConnection.Connection;
            _caches = new ConcurrentDictionary<int, IRhisisCache>();
            _redisConfiguration = redisConfiguration;
        }

        public void ClearAllCaches()
        {
            IEnumerable<CacheType> cacheTypes = Enum.GetValues(typeof(CacheType)).Cast<CacheType>();

            foreach (CacheType cache in cacheTypes)
            {
                ClearCache(cache);
            }
        }

        public void ClearCache(CacheType type)
        {
            IRhisisCache cache = GetCache(type);

            if (cache != null)
            {
                cache.Clear();
            }
        }

        public IRhisisCache GetCache(CacheType type)
        {
            int cacheId = (int)type;

            if (_caches.TryGetValue(cacheId, out IRhisisCache cache))
            {
                return cache;
            }

            cache = new RedisCache(_connection.GetServer(_redisConfiguration.Host, _redisConfiguration.Port), _connection.GetDatabase(cacheId));

            if (!_caches.TryAdd(cacheId, cache))
            {
                throw new InvalidOperationException("Failed to add redis cache connection.");
            }

            return cache;
        }
    }
}
