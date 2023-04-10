using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rhisis.LoginServer.Core;

public sealed class CoreCacheServer : LiteServer<CoreCacheUser>, IClusterCache
{
    private readonly ILogger<CoreCacheServer> _logger;
    private readonly ConcurrentDictionary<string, ClusterInfo> _clusterCache = new();

    public IReadOnlyList<ClusterInfo> Clusters => _clusterCache.Values.ToImmutableList();

    public CoreCacheServer(LiteServerOptions options, ILogger<CoreCacheServer> logger, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Cluster cache server listening on port {Options.Port}.");
    }

    public ClusterInfo GetCluster(string clusterName) => _clusterCache.GetValueOrDefault(clusterName);

    public bool HasCluster(string clusterName) => _clusterCache.ContainsKey(clusterName);

    public void AddCluster(ClusterInfo clusterInfo)
    {
        ArgumentNullException.ThrowIfNull(clusterInfo, nameof(clusterInfo));

        if (!_clusterCache.TryAdd(clusterInfo.Name, clusterInfo))
        {
            throw new InvalidOperationException($"Failed to add cluster '{clusterInfo.Name}' to cache list.");
        }
    }

    public void RemoveCluster(string clusterName)
    {
        ArgumentException.ThrowIfNullOrEmpty(clusterName, nameof(clusterName));

        if (!_clusterCache.TryRemove(clusterName, out _))
        {
            throw new InvalidOperationException($"Failed to remove cluster '{clusterName}' from cache list.");
        }
    }
}
