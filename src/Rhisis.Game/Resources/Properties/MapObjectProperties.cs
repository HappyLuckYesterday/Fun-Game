namespace Rhisis.Game.Resources.Properties;

public sealed class MapObjectProperties
{
    /// <summary>
    /// Gets the map object model id.
    /// </summary>
    public int ModelId { get; init; }

    /// <summary>
    /// Gets the map object position.
    /// </summary>
    public Vector3 Position { get; init; }

    /// <summary>
    /// Gets the map object rotation angle.
    /// </summary>
    public float Angle { get; init; }

    /// <summary>
    /// Gets the map object name.
    /// </summary>
    public string Name { get; init; }
}