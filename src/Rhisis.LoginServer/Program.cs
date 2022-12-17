using LiteNetwork;
using LiteNetwork.Hosting;
using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Abstractions.Protocol;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Infrastructure.Persistance;
using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer;

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
                configApp.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();
                services.Configure<LoginOptions>(hostContext.Configuration.GetSection(ConfigurationSections.Login));
                services.Configure<CoreOptions>(hostContext.Configuration.GetSection(ConfigurationSections.Core));

                services.AddPersistance(hostContext.Configuration);
                services.AddHandlers();
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
                // Login Server
                builder.AddLiteServer<ILoginServer, LoginServer>(options =>
                {
                    var serverOptions = context.Configuration.GetSection(ConfigurationSections.Login).Get<LoginOptions>();

                    if (serverOptions is null)
                    {
                        throw new InvalidProgramException($"Failed to load login server settings.");
                    }

                    options.Host = serverOptions.Host;
                    options.Port = serverOptions.Port;
                    options.PacketProcessor = new FlyffPacketProcessor();
                    options.ReceiveStrategy = ReceiveStrategyType.Queued;
                });

                // Core Server
                builder.AddLiteServer<ICoreServer, CoreServer>(options =>
                {
                    var serverConfiguration = context.Configuration.GetSection(ConfigurationSections.Core).Get<CoreOptions>();

                    if (serverConfiguration is null)
                    {
                        throw new InvalidProgramException($"Failed to load core server settings.");
                    }

                    options.Host = serverConfiguration.Host;
                    options.Port = serverConfiguration.Port;
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