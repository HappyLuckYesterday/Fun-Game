namespace Rhisis.Game.Resources.Properties;

public sealed class MapTriggerRegionProperties : MapRegionProperties
{
    /// <summary>
    /// Gets the trigger destination map id.
    /// </summary>
    public int DestinationMapId { get; init; }

    /// <summary>
    /// Gets the trigger destination map position.
    /// </summary>
    public Vector3 DestinationMapPosition { get; init; }

    /// <summary>
    /// Gets a value that indicates if the current region is wrapzone.
    /// </summary>
    public bool IsWrapzone => DestinationMapId > 0;
}
