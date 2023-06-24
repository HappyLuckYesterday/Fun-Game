using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.WorldServer.Caching;

public class ClusterCacheClient : FFInterClient
{
    private readonly ILogger<ClusterCacheClient> _logger;
    private readonly IOptions<WorldChannelServerOptions> _channelOptions;

    public ClusterCacheClient(LiteClientOptions options, ILogger<ClusterCacheClient> logger, IOptions<WorldChannelServerOptions> channelOptions, IServiceProvider serviceProvider = null)
        : base(options, logger, serviceProvider)
    {
        _logger = logger;
        _channelOptions = channelOptions;
    }

    protected override void OnConnected()
    {
        _logger.LogInformation($"Channel '{_channelOptions.Value.Name}' connected to cluster cache server.");
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation($"Channel '{_channelOptions.Value.Name}' disconnected from cluster cache server.");
    }
}
