using LiteNetwork.Server;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer.Caching;

/// <summary>
/// Provides a mechanism to manage and cache the connected cluster.
/// </summary>
internal sealed class ClusterCache
{
    private readonly List<Cluster> _clusters = new();

    /// <summary>
    /// Gets the connected clusters.
    /// </summary>
    public IReadOnlyList<Cluster> Clusters => _clusters;

    /// <summary>
    /// Gets a cluster by its name.
    /// </summary>
    /// <param name="name">Clsuter name.</param>
    /// <returns>Cluster if found; null otherwise.</returns>
    public Cluster GetCluster(string name) => _clusters.FirstOrDefault(x => x.Name == name);

    /// <summary>
    /// Removes a cluster by its name.
    /// </summary>
    /// <param name="name">Cluster name to remove.</param>
    public void RemoveCluster(string name)
    {
        Cluster cluster = GetCluster(name);

        if (cluster is not null)
        {
            _clusters.Remove(cluster);
        }
    }
}

internal sealed class ClusterCacheServer : LiteServer<ClusterCacheUser>
{
    public IReadOnlyList<ClusterCacheUser> Clusters => Users.Cast<ClusterCacheUser>().ToList();

    public ClusterCacheServer(LiteServerOptions options, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
    }
}

internal sealed class ClusterCacheUser : LiteServerUser
{
}