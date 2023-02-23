using LiteNetwork;
using LiteNetwork.Client.Hosting;
using LiteNetwork.Hosting;
using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Configuration;
using Rhisis.Core.Extensions;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol;
using System;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer;

internal class Program
{
    public static async Task Main()
    {
        const string culture = "en-US";

        var host = new HostBuilder()
           .ConfigureAppConfiguration((hostContext, config) =>
           {
               config.AddEnvironmentVariables();
               config.SetBasePath(EnvironmentExtension.GetCurrentEnvironementDirectory());
               config.AddYamlFile("config/login-server.yml", optional: false, reloadOnChange: true);
           })
           .ConfigureServices((hostContext, services) =>
           {
               services.AddOptions();
               services.Configure<LoginServerOptions>(hostContext.Configuration.GetSection("server"));
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
               builder.AddLiteServer<ClusterServer>(options =>
               {
                   var serverOptions = context.Configuration.GetSection("server").Get<LoginServerOptions>();

                   if (serverOptions is null)
                   {
                       throw new InvalidProgramException($"Failed to load login server settings.");
                   }

                   options.Host = serverOptions.Ip;
                   options.Port = serverOptions.Port;
                   options.PacketProcessor = new FlyffPacketProcessor();
                   options.ReceiveStrategy = ReceiveStrategyType.Queued;
               });
               builder.AddLiteClient<ClusterCacheClient>(options =>
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
