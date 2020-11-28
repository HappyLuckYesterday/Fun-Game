using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Messaging.RabbitMQ;
using Sylver.HandlerInvoker;
using System.Threading.Tasks;

namespace Rhisis.CoreServer
{
    class Program
    {
        static Task Main()
        {
            const string culture = "en-US";

            var host = new HostBuilder()
                   .ConfigureAppConfiguration((hostContext, configApp) =>
                   {
                       configApp.SetBasePath(EnvironmentExtension.GetCurrentEnvironementDirectory());
                       configApp.AddJsonFile(ConfigurationConstants.CoreServerPath, optional: false);
                   })
                   .ConfigureServices((hostContext, services) =>
                   {
                       services.AddOptions();
                       services.Configure<CoreConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.CoreServer));
                       services.AddHandlers();
                       services.AddSingleton<ICoreServer, CoreServer>();
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
                   .UseRabbitMQ((host, options) =>
                   {
                       // TODO: load from configuration
                       options.Host = "rhisis.messagequeue";
                       options.Username = "rabbitmq";
                       options.Password = "rabbitmq";
                   })
                   .UseConsoleLifetime()
                   .SetConsoleCulture(culture)
                   .Build();

            return host.RunAsync();
        }
    }
}
