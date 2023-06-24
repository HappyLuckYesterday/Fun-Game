using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.LoginServer.Core;

public sealed class CoreCacheUser : FFInterServerConnection
{
    private readonly ILogger<CoreCacheUser> _logger;

    public bool IsAuthenticated => Cluster is not null;

    public ClusterInfo Cluster { get; set; }

    public CoreCacheUser(ILogger<CoreCacheUser> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnConnected()
    {
        _logger.LogInformation($"New cluster connected to core server ({Id}).");

        base.OnConnected();
    }
}