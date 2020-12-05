using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.LoginServer.CoreServer;
using Rhisis.LoginServer.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using System.Threading.Tasks;

namespace Rhisis.LoginServer
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
                    configApp.AddJsonFile(ConfigurationConstants.LoginServerPath, optional: false);
                    configApp.AddJsonFile(ConfigurationConstants.CoreServerPath, optional: false);
                    configApp.AddJsonFile(ConfigurationConstants.DatabasePath, optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<LoginConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.LoginServer));
                    services.Configure<CoreConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.CoreServer));

                    services.AddDatabase(hostContext.Configuration);
                    services.AddHandlers();

                    // Login Server
                    services.AddSingleton<ILoginServer, LoginServer>();
                    services.AddSingleton<ILoginPacketFactory, LoginPacketFactory>();
                    services.AddSingleton<IHostedService, LoginServerService>();

                    // Core Server
                    services.AddSingleton<ICoreServer, CoreServer.CoreServer>();
                    services.AddSingleton<IHostedService, CoreServerService>();
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
                .UseConsoleLifetime()
                .SetConsoleCulture(culture)
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