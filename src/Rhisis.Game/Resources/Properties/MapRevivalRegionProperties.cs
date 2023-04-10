namespace Rhisis.Game.Resources.Properties;

public sealed class MapRevivalRegionProperties : MapRegionProperties
{
    /// <summary>
    /// Gets the revival region's map id.
    /// </summary>
    public int MapId { get; init; }

    /// <summary>
    /// Gets the revival region's map key.
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// Gets a value that indicates if the region is a revival region for player that has killed other players.
    /// Related to the PK Mode. (Chao mode)
    /// </summary>
    public bool IsChaoRegion { get; init; }

    /// <summary>
    /// Gets a value that indicates if the current region has to target another revival key.
    /// </summary>
    public bool TargetRevivalKey { get; init; }

    /// <summary>
    /// Gets the revival position.
    /// </summary>
    public Vector3 RevivalPosition { get; init; }
}