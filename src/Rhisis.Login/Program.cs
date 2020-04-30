﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Login.Core;
using Rhisis.Login.Core.Packets;
using Rhisis.Login.Packets;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using System.IO;
using System.Threading.Tasks;

namespace Rhisis.Login
{
    public static class Program
    {
        private static async Task Main()
        {
            const string culture = "en-US";

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile(ConfigurationConstants.LoginServerPath, optional: false);
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
                    services.AddSingleton<ICoreServer, CoreServer>();
                    services.AddSingleton<ICorePacketFactory, CorePacketFactory>();
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