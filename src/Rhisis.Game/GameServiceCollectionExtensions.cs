﻿using Microsoft.Extensions.DependencyInjection;
using Rhisis.Abstractions.Map;
using Rhisis.Abstractions.Resources;
using Rhisis.Game.Map;
using Rhisis.Game.Resources;

namespace Rhisis.Game
{
    public static class GameServiceCollectionExtensions
    {
        public static IServiceCollection AddGameSystems(this IServiceCollection services)
        {
            services.AddSingleton<IGameResources, GameResources>();
            services.AddSingleton<IMapManager, MapManager>();

            return services;
        }
    }
}
