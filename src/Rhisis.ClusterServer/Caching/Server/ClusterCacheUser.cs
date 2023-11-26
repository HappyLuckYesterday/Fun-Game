using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.ClusterServer.Caching.Server;

public class ClusterCacheUser : FFInterServerConnection
{
    private readonly ILogger<ClusterCacheUser> _logger;
    private readonly ICluster _cluster;

    public string Name { get; set; }

    public bool IsAuthenticated { get; set; }

    public ClusterCacheUser(ILogger<ClusterCacheUser> logger, ICluster cluster, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
        _logger = logger;
        _cluster = cluster;
    }

    protected override void OnConnected()
    {
        _logger.LogInformation($"New world channel connected to cluster server ({Id}).");

        base.OnConnected();
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation($"World channel '{Name}' disconnected.");

        if (IsAuthenticated)
        {
            _cluster.RemoveChannel(Name);

            IsAuthenticated = false;
        }
    }
}
