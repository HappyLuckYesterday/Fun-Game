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
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol;
using Rhisis.WorldServer.Abstractions;
using Rhisis.WorldServer.ClusterCache;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.WorldServer;

public static class Program
{
    private static async Task Main()
    {
        const string culture = "en-US";

        var host = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.SetBasePath(EnvironmentExtension.GetCurrentEnvironementDirectory());
                configApp.AddJsonFile(ConfigurationConstants.WorldServerPath, optional: false);
                configApp.AddJsonFile(ConfigurationConstants.DatabasePath, optional: false);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();
                services.AddMemoryCache();
                services.Configure<WorldOptions>(hostContext.Configuration.GetSection(ConfigurationSections.World));
                services.AddPersistance(hostContext.Configuration);
                services.AddHandlers();
                services.AddInjectableServices();
                services.AddGameSystems();
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
                builder.AddLiteServer<IWorldServer, WorldServer>(options =>
                {
                    var serverOptions = context.Configuration.GetSection(ConfigurationSections.World).Get<WorldOptions>();

                    if (serverOptions is null)
                    {
                        throw new InvalidProgramException($"Failed to load world server settings.");
                    }

                    options.Host = serverOptions.Host;
                    options.Port = serverOptions.Port;
                    options.PacketProcessor = new FlyffPacketProcessor();
                    options.ReceiveStrategy = ReceiveStrategyType.Queued;
                });
                builder.AddLiteClient<IClusterCacheClient, ClusterCacheClient>(options =>
                {
                    var serverOptions = context.Configuration.GetSection(ConfigurationSections.World).Get<WorldOptions>();

                    if (serverOptions is null)
                    {
                        throw new InvalidProgramException($"Failed to load world server settings.");
                    }

                    options.Host = serverOptions.ClusterCache.Host;
                    options.Port = serverOptions.ClusterCache.Port;
                    options.ReceiveStrategy = ReceiveStrategyType.Queued;
                });
            })
            .UseConsoleLifetime()
            .SetConsoleCulture(culture)
            .Build();

        await host
            .AddHandlerParameterTransformer<IFFPacket, IPacketDeserializer>((source, dest) =>
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