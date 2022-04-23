using Microsoft.Extensions.DependencyInjection;
using Rhisis.Abstractions.Caching;

namespace Rhisis.Infrastructure.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRhisisCache<>), typeof(RhisisCache<>));

            return services;
        }
    }
}
