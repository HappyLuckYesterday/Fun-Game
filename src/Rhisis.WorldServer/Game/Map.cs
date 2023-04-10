using Rhisis.Game;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Game;

internal class Map
{
    private static readonly int DefaultMapLayerId = 1;
    private static readonly int MapRegionSize = 128;
    private readonly MapLayer _defaultMapLayer;
    private readonly List<MapLayer> _layers = new();

    /// <summary>
    /// Gets the map id.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the map name.
    /// </summary>
    public  string Name { get; }

    /// <summary>
    /// Gets the map bounds.
    /// </summary>
    public Rectangle Bounds { get; }

    public Map(int mapId)
    {
        Id = mapId;
    }

    public MapLayer GetDefaultLayer() => _layers.Single(x => x.Id == DefaultMapLayerId);
}
