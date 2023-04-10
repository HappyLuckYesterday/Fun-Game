using Rhisis.Core.Configuration.Cluster;
using System.Collections.Generic;

namespace Rhisis.ClusterServer.Abstractions;

/// <summary>
/// Provides a mechanism to manage the cluster.
/// </summary>
public interface ICluster
{
    /// <summary>
    /// Gets the cluster name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the cluster server options.
    /// </summary>
    ClusterServerOptions Configuration { get; }

    /// <summary>
    /// Gets a list of the connected world channels.
    /// </summary>
    IReadOnlyList<WorldChannel> Channels { get; }

    /// <summary>
    /// Adds a new world channel to the current cluster.
    /// </summary>
    /// <param name="channel"></param>
    void AddChannel(WorldChannel channel);

    /// <summary>
    /// Removes an existing world channel from the cluster identified by its name.
    /// </summary>
    /// <param name="channelName">Channel name</param>
    void RemoveChannel(string channelName);

    /// <summary>
    /// Gets a world channel by its name.
    /// </summary>
    /// <param name="channelName">Channel name</param>
    /// <returns>World channel instance matchin the given name; null if not found.</returns>
    WorldChannel GetChannel(string channelName);

    /// <summary>
    /// Gets a world channel by its id.
    /// </summary>
    /// <param name="channelId">Channel id.</param>
    /// <returns>World channel instance matching the given id; null if not found.</returns>
    WorldChannel GetChannel(int channelId);

    /// <summary>
    /// Checks if a channel with the given name exists in the cluster.
    /// </summary>
    /// <param name="channelName">Channel name.</param>
    /// <returns>True if the world channel exists; false otherwise.</returns>
    bool HasChannel(string channelName);

    /// <summary>
    /// Sends the channels information to the core cache server.
    /// </summary>
    void SendChannels();
}