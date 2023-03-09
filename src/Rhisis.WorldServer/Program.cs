using LiteNetwork;
using LiteNetwork.Client.Hosting;
using LiteNetwork.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Configuration;
using Rhisis.Core.Extensions;
using Rhisis.Infrastructure.Persistance;
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
               builder.SetMinimumLevel(LogLevel.Trace);
               builder.AddFilter("LiteNetwork.*", LogLevel.Warning);
               builder.AddFilter("Microsoft.EntityFrameworkCore.*", LogLevel.Warning);
               builder.AddFilter("Microsoft.Extensions.*", LogLevel.Warning);
               builder.AddFilter("Microsoft.Hosting.*", LogLevel.Warning);
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
           })
           .UseConsoleLifetime()
           .SetConsoleCulture(culture)
           .Build();

        await host.RunAsync();
    }
}
