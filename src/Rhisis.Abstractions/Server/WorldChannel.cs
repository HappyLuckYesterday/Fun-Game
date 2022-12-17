namespace Rhisis.Abstractions.Server;

/// <summary>
/// Defines the world channel data structure.
/// </summary>
public class WorldChannel : BaseServer
{
    /// <summary>
    /// Gets or sets the number of maximum connected users allowed.
    /// </summary>
    public int MaximumUsers { get; set; }

    /// <summary>
    /// Gets or sets the number of connected users on the current channel.
    /// </summary>
    public int ConnectedUsers { get; set; }
}
