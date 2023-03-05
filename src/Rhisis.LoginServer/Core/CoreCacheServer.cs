using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer.Core;

public sealed class CoreCacheServer : LiteServer<CoreCacheUser>, IClusterCache
{
    private readonly ILogger<CoreCacheServer> _logger;

    public CoreCacheServer(LiteServerOptions options, ILogger<CoreCacheServer> logger, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Cluster cache server listening on port {Options.Port}.");
    }

    public IReadOnlyList<ClusterInfo> GetClusters() => Users.Cast<CoreCacheUser>().Where(x => x.Cluster is not null).Select(x => x.Cluster).ToList();

    public ClusterInfo GetCluster(string clusterName) => GetClusters().SingleOrDefault(x => x.Name == clusterName);

    public bool HasCluster(string clusterName) => GetCluster(clusterName) is not null;
}
