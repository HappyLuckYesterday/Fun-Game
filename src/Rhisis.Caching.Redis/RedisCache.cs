using Rhisis.Caching.Abstractions;
using StackExchange.Redis;
using System.Text.Json;

namespace Rhisis.Caching.Redis
{
    internal class RedisCache : IRhisisCache
    {
        private readonly IDatabase _redisDatabase;

        public RedisCache(IDatabase database)
        {
            _redisDatabase = database;
        }

        public bool Contains(string key)
        {
            return _redisDatabase.StringGet(key).HasValue;
        }

        public bool Delete(string key)
        {
            return _redisDatabase.KeyDelete(key);
        }

        public TObject Get<TObject>(string key) where TObject : class, new()
        {
            RedisValue content = _redisDatabase.StringGet(key);

            if (!content.HasValue)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TObject>(content);
        }

        public bool Set<TObject>(string key, TObject @object)
        {
            return _redisDatabase.StringSet(key, JsonSerializer.Serialize(@object));
        }
    }
}
