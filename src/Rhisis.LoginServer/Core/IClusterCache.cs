using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.LoginServer.Core;

/// <summary>
/// Provides a mechanism to manage the cluster cache.
/// </summary>
public interface IClusterCache
{
    /// <summary>
    /// Gets the connected clusters.
    /// </summary>
    /// <returns>A collection of connected clusters.</returns>
    IReadOnlyList<ClusterInfo> Clusters { get; }

    /// <summary>
    /// Gets the cluster information by its name.
    /// </summary>
    /// <param name="clusterName">Cluster name.</param>
    /// <returns>Cluster information matching the given name.</returns>
    ClusterInfo GetCluster(string clusterName);

    /// <summary>
    /// Adds a new cluster to the cache.
    /// </summary>
    /// <param name="clusterInfo">Cluster info</param>
    void AddCluster(ClusterInfo clusterInfo);

    /// <summary>
    /// Removes a cluster from the cache.
    /// </summary>
    /// <param name="clusterName">Cluster name.</param>
    void RemoveCluster(string clusterName);

    /// <summary>
    /// Checks if there is a cluster already connected with the same name.
    /// </summary>
    /// <param name="clusterName">Cluster name.</param>
    /// <returns>True if the cluster is already connected; false otherwise.</returns>
    bool HasCluster(string clusterName);
}
