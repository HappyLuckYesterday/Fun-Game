using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.ClusterServer.Caching;

/// <summary>
/// Provides a mechanism to manage the world channels cache.
/// </summary>
public interface IWorldChannelCache
{
    /// <summary>
    /// Gets all world channels connected.
    /// </summary>
    /// <returns>Collection of world channels connected to the cluster cache.</returns>
    IReadOnlyList<WorldChannelInfo> GetWorldChannels();

    /// <summary>
    /// Gets a world channel going by the given name.
    /// </summary>
    /// <param name="channelName">Channel name to retrieve.</param>
    /// <returns>The world channel information if found; null otherwise.</returns>
    WorldChannelInfo GetWorldChannel(string channelName);
}