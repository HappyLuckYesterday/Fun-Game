using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Game;

public class MapManager : Singleton<MapManager>
{
    private readonly ConcurrentDictionary<int, Map> _maps = new();

    public void Initialize(IServiceProvider serviceProvider)
    {
        IEnumerable<MapProperties> properties = GameResources.Current.Maps.GetAll();

        foreach (MapProperties mapProperties in properties)
        {
            Map map = new(mapProperties, serviceProvider.GetRequiredService<ILogger<Map>>());

            _maps.TryAdd(map.Id, map);
        }
    }

    public Map Get(int mapId) => _maps.GetValueOrDefault(mapId);
}