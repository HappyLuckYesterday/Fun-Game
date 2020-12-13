using Rhisis.Game.Abstractions.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace Rhisis.Redis.Caching
{
    internal class RedisCache : IRhisisCache
    {
        private readonly IServer _server;
        private readonly IDatabase _redisDatabase;

        public RedisCache(IServer server, IDatabase database)
        {
            _server = server;
            _redisDatabase = database;
        }

        public bool Contains(string key)
        {
            return _redisDatabase.StringGet(key).HasValue;
        }

        public void Clear()
        {
            _server.FlushDatabase(_redisDatabase.Database);
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
