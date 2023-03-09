using System;

namespace Rhisis.ClusterServer;

/// <summary>
/// Provides a data structure that reprents a world channel.
/// </summary>
public sealed class WorldChannel
{
    /// <summary>
    /// Gets the world channel connection id.
    /// </summary>
    public Guid ConnectionId { get; }

    /// <summary>
    /// Gets the world channel id.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the world channel IP address.
    /// </summary>
    public string Ip { get; init; }

    /// <summary>
    /// Gets the world channel listening port.
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Gets the world channel name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the world channel maximum users.
    /// </summary>
    public int MaximumUsers { get; init; }

    /// <summary>
    /// Gets the world channel connected user amount.
    /// </summary>
    public int ConnectedUsers { get; init; }

    /// <summary>
    /// Gets a boolean value that indicates if the world channel is enabled.
    /// </summary>
    public bool IsEnabled { get; init; }

    /// <summary>
    /// Creates a new <see cref="WorldChannel"/> instance.
    /// </summary>
    /// <param name="connectionId"></param>
    internal WorldChannel(Guid connectionId)
    {
        ConnectionId = connectionId;
    }
}
