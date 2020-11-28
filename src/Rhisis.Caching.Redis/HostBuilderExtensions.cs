using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using StackExchange.Redis;
using Rhisis.Caching.Abstractions;

namespace Rhisis.Caching.Redis
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseRedisCache(this IHostBuilder hostBuilder, Action<HostBuilderContext, RedisConfiguration> builder)
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
                services.AddSingleton<IRhisisCacheManager>(serviceProvider =>
                {
                    return ActivatorUtilities.CreateInstance<RedisCacheManager>(serviceProvider);
                });
            });

            return hostBuilder;
        }
    }
}
