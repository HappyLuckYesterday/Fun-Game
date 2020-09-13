using Microsoft.Extensions.DependencyInjection;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Resources;

namespace Rhisis.Game
{
    public static class GameServiceCollectionExtensions
    {
        public static IServiceCollection AddGameSystems(this IServiceCollection services)
        {
            services.AddSingleton<IGameResources, GameResources>();

            return services;
        }
    }
}
