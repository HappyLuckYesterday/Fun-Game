using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rhisis.Messaging.Abstractions;
using System;

namespace Rhisis.Messaging.RabbitMQ
{
    /// <summary>
    /// Provides extensions to setup the rabbit MQ messaging system into a host builder.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Initializes the RabbitMQ messaging system.
        /// </summary>
        /// <param name="hostBuilder">Current host builder.</param>
        /// <param name="configuration">Rabbit MQ configuration.</param>
        /// <returns></returns>
        public static IHostBuilder UseRabbitMQ(this IHostBuilder hostBuilder, Action<HostBuilderContext, RabbitMQBuilderOptions> builder)
        {
            hostBuilder.ConfigureServices((host, services) =>
            {
                services.AddSingleton<IMessaging, RabbitMQMessaging>(provider =>
                {
                    var options = new RabbitMQBuilderOptions();

                    if (builder != null)
                    {
                        builder(host, options);
                    }

                    return new RabbitMQMessaging(options);
                });
            });

            return hostBuilder;
        }
    }
}
