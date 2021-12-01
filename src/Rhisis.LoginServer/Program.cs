using LiteNetwork;
using LiteNetwork.Hosting;
using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Infrastructure.Persistance;
using Rhisis.LoginServer.Abstractions;
using Rhisis.LoginServer.Core;
using Rhisis.LoginServer.Core.Abstractions;
using Rhisis.LoginServer.Packets;
using Rhisis.Protocol;
using Rhisis.Protocol.Abstractions;
using Sylver.HandlerInvoker;
using System;
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
                    configApp.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<LoginConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.LoginServer));
                    services.Configure<CoreConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.CoreServer));

                    services.AddPersistance(hostContext.Configuration);
                    services.AddHandlers();
                    services.AddSingleton<ILoginPacketFactory, LoginPacketFactory>();
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
                    builder.AddLiteServer<ILoginServer, LoginServer, LoginClient>(options =>
                    {
                        var serverOptions = context.Configuration.GetSection(ConfigurationConstants.LoginServer).Get<LoginConfiguration>();

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
                    builder.AddLiteServer<ICoreServer, CoreServer, CoreServerClient>(options =>
                    {
                        var serverConfiguration = context.Configuration.GetSection(ConfigurationConstants.CoreServer).Get<CoreConfiguration>();

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
                .AddHandlerParameterTransformer<ILitePacketStream, IPacketDeserializer>((source, dest) =>
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