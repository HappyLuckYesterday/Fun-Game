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

internal class Program
{
    static async Task Main(string[] args)
    {
        const string culture = "en-US";

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
               //services.Configure<ClusterServerOptions>(hostContext.Configuration.GetSection("server"));
               services.Configure<ClusterCacheOptions>(hostContext.Configuration.GetSection("cluster-cache-server"));

               services.AddAccountPersistance(hostContext.Configuration.GetSection("account-database").Get<DatabaseOptions>());
               services.AddGamePersistance(hostContext.Configuration.GetSection("game-database").Get<DatabaseOptions>());
           })
           .ConfigureLogging(builder =>
           {
               builder.SetMinimumLevel(LogLevel.Trace);
               builder.AddConsole();
           })
           .ConfigureLiteNetwork((context, builder) =>
           {
               builder.AddLiteClient<WorldChannelCacheUser>(options =>
               {
                   var cacheClientOptions = context.Configuration.GetSection("cache").Get<ClusterCacheOptions>();

                   if (cacheClientOptions is null)
                   {
                       throw new InvalidProgramException("Failed to load cluster cache client settings.");
                   }

                   options.Host = cacheClientOptions.Ip;
                   options.Port = cacheClientOptions.Port;
                   options.ReceiveStrategy = ReceiveStrategyType.Queued;
               });
           })
           .UseConsoleLifetime()
           .SetConsoleCulture(culture)
           .Build();

        await host.RunAsync();
    }
}
