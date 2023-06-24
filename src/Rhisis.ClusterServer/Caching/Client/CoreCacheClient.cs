using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration.Cluster;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.ClusterServer.Caching;

public sealed class CoreCacheClient : FFInterClient
{
    private readonly ILogger<CoreCacheClient> _logger;
    private readonly IOptions<ClusterServerOptions> _clusterOptions;

    public string Name => _clusterOptions.Value.Name;

    public CoreCacheClient(LiteClientOptions options,
        ILogger<CoreCacheClient> logger,
        IOptions<ClusterServerOptions> clusterOptions,
        IServiceProvider serviceProvider = null)
        : base(options, logger, serviceProvider)
    {
        _logger = logger;
        _clusterOptions = clusterOptions;
    }

    protected override void OnConnected()
    {
        _logger.LogInformation($"Cluster '{_clusterOptions.Value.Name}' connected to Core Cache server.");
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation($"Cluster '{_clusterOptions.Value.Name}' disconnected from Core Cache server.");
    }
}
