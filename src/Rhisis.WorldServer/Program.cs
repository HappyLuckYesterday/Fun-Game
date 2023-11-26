using LiteNetwork;
using LiteNetwork.Client.Hosting;
using LiteNetwork.Hosting;
using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Configuration;
using Rhisis.Core.Configuration.Cluster;
using Rhisis.Core.Extensions;
using Rhisis.Game.Chat;
using Rhisis.Game.Resources;
using Rhisis.Infrastructure.Logging;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol;
using Rhisis.WorldServer.Abstractions;
using Rhisis.WorldServer.Caching;
using System;
using System.Threading.Tasks;

namespace Rhisis.WorldServer;

internal static class Program
{
    static async Task Main(string[] args)
    {
        const string culture = "en-US";

        Console.Title = "Rhisis - World Server";
        var host = new HostBuilder()
           .ConfigureAppConfiguration((_, config) =>
           {
               config.AddEnvironmentVariables();
               config.SetBasePath(EnvironmentExtension.GetCurrentEnvironementDirectory());
               config.AddYamlFile("config/world-server.yml", optional: false, reloadOnChange: true);
           })
           .ConfigureServices((hostContext, services) =>
           {
               services.AddOptions();
               services.Configure<WorldChannelServerOptions>(hostContext.Configuration.GetSection("server"));

               services.AddAccountPersistance(hostContext.Configuration.GetSection("account-database").Get<DatabaseOptions>());
               services.AddGamePersistance(hostContext.Configuration.GetSection("game-database").Get<DatabaseOptions>());
           })
           .ConfigureLogging(builder =>
           {
               builder.AddConsole();
               builder.AddLoggingFilters();
           })
           .ConfigureLiteNetwork((context, builder) =>
           {
               builder.AddLiteClient<ClusterCacheClient>(options =>
               {
                   var cacheClientOptions = context.Configuration.GetSection("server").Get<WorldChannelServerOptions>();

                   if (cacheClientOptions is null)
                   {
                       throw new InvalidProgramException("Failed to load cluster cache client settings.");
                   }

                   options.Host = cacheClientOptions.Cluster.Ip;
                   options.Port = cacheClientOptions.Cluster.Port;
                   options.ReceiveStrategy = ReceiveStrategyType.Queued;
               });
               builder.AddLiteServer<IWorldChannel, WorldServer>(options =>
               {
                   var serverOptions = context.Configuration.GetSection("server").Get<WorldChannelServerOptions>();

                   if (serverOptions is null)
                   {
                       throw new InvalidProgramException($"Failed to load world server settings.");
                   }

                   options.Host = serverOptions.Ip;
                   options.Port = serverOptions.Port;
                   options.PacketProcessor = new FlyffPacketProcessor();
                   options.ReceiveStrategy = ReceiveStrategyType.Queued;
               });
           })
           .UseConsoleLifetime()
           .SetConsoleCulture(culture)
           .Build();

        GameResources.Current.Initialize(host.Services);
        GameResources.Current.Items.Load();
        GameResources.Current.Movers.Load();
        GameResources.Current.Npcs.Load();
        GameResources.Current.Skills.Load();
        GameResources.Current.Jobs.Load();
        GameResources.Current.ExperienceTable.Load();
        GameResources.Current.Penalities.Load();
        GameResources.Current.Quests.Load();
        ChatCommandManager.Current.Load(host.Services);

        await host.RunAsync();
    }
}
