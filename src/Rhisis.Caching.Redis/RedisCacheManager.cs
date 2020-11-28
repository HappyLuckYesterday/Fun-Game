using Rhisis.Caching.Abstractions;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace Rhisis.Caching.Redis
{
    internal class RedisCacheManager : IRhisisCacheManager
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly ConcurrentDictionary<int, IRhisisCache> _caches;

        public RedisCacheManager(RedisConfiguration configuration)
        {
            var redisConfiguration = ConfigurationOptions.Parse(configuration.Host);

            _connection = ConnectionMultiplexer.Connect(redisConfiguration);
            _caches = new ConcurrentDictionary<int, IRhisisCache>();
        }

        public IRhisisCache GetCache(int cacheId)
        {
            if (_caches.TryGetValue(cacheId, out IRhisisCache cache))
            {
                return cache;
            }

            cache = new RedisCache(_connection.GetDatabase(cacheId));

            if (!_caches.TryAdd(cacheId, cache))
            {
                throw new InvalidOperationException("Failed to add redis cache connection.");
            }

            return cache;
        }
    }
}
