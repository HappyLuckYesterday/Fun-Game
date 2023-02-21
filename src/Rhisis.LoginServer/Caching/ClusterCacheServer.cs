using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer.Caching;

public sealed class ClusterCacheServer : LiteServer<ClusterCacheUser>
{
    private readonly ILogger<ClusterCacheServer> _logger;

    public IReadOnlyList<Cluster> Clusters => Users.Cast<ClusterCacheUser>().Where(x => x.Cluster is not null).Select(x => x.Cluster).ToList();

    public ClusterCacheServer(LiteServerOptions options, ILogger<ClusterCacheServer> logger, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Cluster cache server listening on port {Options.Port}.");
    }

    public Cluster GetCluster(string name) => Clusters.SingleOrDefault(x => x.Name == name);
}
