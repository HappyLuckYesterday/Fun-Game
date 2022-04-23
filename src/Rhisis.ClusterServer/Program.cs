using LiteNetwork;
using LiteNetwork.Client.Hosting;
using LiteNetwork.Hosting;
using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Abstractions.Protocol;
using Rhisis.Abstractions.Resources;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Cache;
using Rhisis.ClusterServer.Core;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Game.Resources;
using Rhisis.Infrastructure.Caching;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol;
using Sylver.HandlerInvoker;
using System;
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
                    configApp.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddMemoryCache();
                    services.Configure<ClusterOptions>(hostContext.Configuration.GetSection(ConfigurationSections.Cluster));
                    services.AddPersistance(hostContext.Configuration);
                    services.AddCache();
                    services.AddHandlers();
                    services.AddSingleton<IGameResources, GameResources>();
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
                .ConfigureLiteNetwork((context, builder) =>
                {
                    builder.AddLiteServer<IClusterServer, ClusterServer>(options =>
                    {
                        var serverOptions = context.Configuration.GetSection(ConfigurationSections.Cluster).Get<ClusterOptions>();

                        if (serverOptions is null)
                        {
                            throw new InvalidProgramException($"Failed to load cluster server settings.");
                        }

                        options.Host = serverOptions.Host;
                        options.Port = serverOptions.Port;
                        options.PacketProcessor = new FlyffPacketProcessor();
                        options.ReceiveStrategy = ReceiveStrategyType.Queued;
                    });

                    builder.AddLiteServer<IClusterCacheServer, ClusterCacheServer>(options =>
                    {
                        var serverOptions = context.Configuration.GetSection(ConfigurationSections.Cluster).Get<ClusterOptions>();

                        if (serverOptions is null)
                        {
                            throw new InvalidProgramException($"Failed to load cluster server settings.");
                        }

                        if (serverOptions.Cache is null)
                        {
                            throw new InvalidOperationException("Failed to load cluster cache server settings.");
                        }

                        options.Host = serverOptions.Cache.Host;
                        options.Port = serverOptions.Cache.Port;
                        options.ReceiveStrategy = ReceiveStrategyType.Queued;
                    });

                    builder.AddLiteClient<ClusterCoreClient>(options =>
                    {
                        var serverOptions = context.Configuration.GetSection(ConfigurationSections.Cluster).Get<ClusterOptions>();

                        if (serverOptions is null)
                        {
                            throw new InvalidProgramException($"Failed to load cluster server settings.");
                        }

                        if (serverOptions.Core is null)
                        {
                            throw new InvalidOperationException("Failed to load core server settings.");
                        }

                        options.Host = serverOptions.Core.Host;
                        options.Port = serverOptions.Core.Port;
                        options.ReceiveStrategy = ReceiveStrategyType.Queued;
                    });
                })
                .SetConsoleCulture(culture)
                .UseConsoleLifetime()
                .Build();

            await host
                .AddHandlerParameterTransformer<FFPacket, IPacketDeserializer>((source, dest) =>
                {
                    if (source is not IFFPacket packet)
                    {
                        throw new InvalidCastException("Failed to convert a lite packet stream into a Flyff packet stream.");
                    }

                    dest?.Deserialize(packet);
                    return dest;
                })
                .RunAsync();
        }
    }
}