using Rhisis.Game.Common;

namespace Rhisis.Game.Resources.Properties;

public sealed class MapRespawnRegionProperties : MapRegionProperties
{
    /// <summary>
    /// Gets the respawn region object type.
    /// </summary>
    public WorldObjectType ObjectType { get; init; }

    /// <summary>
    /// Gets the respawn model id.
    /// </summary>
    public int ModelId { get; init; }

    /// <summary>
    /// Gets the respawn time.
    /// </summary>
    public int Time { get; init; }

    /// <summary>
    /// Gets the number of object of this region.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Gets the region height.
    /// </summary>
    /// <remarks>
    /// Used for flying monsters.
    /// </remarks>
    public float Height { get; init; }
}
