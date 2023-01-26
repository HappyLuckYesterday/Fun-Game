using LiteMessageHandler;
using LiteNetwork;
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
using Rhisis.Protocol.Handlers;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer;

internal static class Program
{
    static async Task Main(string[] args)
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

               services.AddAccountPersistance(hostContext.Configuration.GetSection("database").Get<DatabaseOptions>());
               services.AddSingleton<IMessageHandlerDispatcher, MessageHandlerDispatcher>();
           })
           .ConfigureLogging(builder =>
           {
               builder.AddFilter("Microsoft", LogLevel.Warning);
               builder.SetMinimumLevel(LogLevel.Trace);
               builder.AddConsole();
               //builder.AddNLog(new NLogProviderOptions
               //{
               //    CaptureMessageTemplates = true,
               //    CaptureMessageProperties = true
               //});
           })
           .ConfigureLiteNetwork((context, builder) =>
           {
               builder.AddLiteServer<LoginServer>(options =>
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
           })
           .UseConsoleLifetime()
           .SetConsoleCulture(culture)
           .Build();

        PacketHandlerCache.Load(typeof(IMessageHandler<>));

        await host.RunAsync();
    }
}
