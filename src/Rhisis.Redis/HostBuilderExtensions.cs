using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Redis.Caching;
using Rhisis.Redis.Internal;
using Rhisis.Redis.Messaging;
using System;

namespace Rhisis.Redis
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseRedis(this IHostBuilder hostBuilder, Action<HostBuilderContext, RedisConfiguration> builder)
        {
            if (hostBuilder is null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                var configuration = new RedisConfiguration();
                builder(hostContext, configuration);

                services.AddSingleton(configuration);
                services.AddSingleton<RedisConnection>();
                services.AddSingleton<IRhisisCacheManager, RedisCacheManager>();
                services.AddSingleton<IMessaging, RedisMessaging>();
            });

            return hostBuilder;
        }
    }
}
