using StackExchange.Redis;
using System;

namespace Rhisis.Redis.Internal
{
    internal class RedisConnection : IDisposable
    {
        public ConnectionMultiplexer Connection { get; }

        public RedisConnection(RedisConfiguration configuration)
        {
            var redisConfiguration = ConfigurationOptions.Parse(configuration.Host);

            Connection = ConnectionMultiplexer.Connect(redisConfiguration);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
