﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.ClusterServer.Core;
using Rhisis.ClusterServer.Packets;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Caching.Redis;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Resources;
using Rhisis.Messaging.RabbitMQ;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer
{
    public static class Program
    {
        private static async Task Main()
        {
            const string culture = "en-US";

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(EnvironmentExtension.GetCurrentEnvironementDirectory());
                    configApp.AddJsonFile(ConfigurationConstants.ClusterServerPath, optional: false);
                    configApp.AddJsonFile(ConfigurationConstants.DatabasePath, optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddMemoryCache();

                    services.Configure<ClusterConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.ClusterServer));
                    services.Configure<CoreConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.CoreServer));

                    services.AddDatabase(hostContext.Configuration);
                    services.AddHandlers();
                    services.AddSingleton<IGameResources, GameResources>();

                    // Cluster server configuration
                    services.AddSingleton<IClusterServer, ClusterServer>();
                    services.AddSingleton<IClusterPacketFactory, ClusterPacketFactory>();
                    services.AddSingleton<IHostedService, ClusterServerService>();

                    // Core server
                    services.AddSingleton<IHostedService, ClusterCoreClient>();
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning);
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                })
                .UseRabbitMQ((host, options) =>
                {
                    options.Host = "rhisis.messagequeue";
                    options.Username = "rabbitmq";
                    options.Password = "rabbitmq";
                })
                .UseRedisCache((host, options) =>
                {
                    options.Host = "rhisis.redis";
                })
                .SetConsoleCulture(culture)
                .UseConsoleLifetime()
                .Build();

            await host
                .AddHandlerParameterTransformer<INetPacketStream, IPacketDeserializer>((source, dest) =>
                {
                    dest?.Deserialize(source);
                    return dest;
                })
                .RunAsync();
        }
    }
}