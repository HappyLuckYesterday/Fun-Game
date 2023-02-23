using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using System;

namespace Rhisis.ClusterServer;

public sealed class ClusterServer : LiteServer<ClusterUser>
{
    private readonly ILogger<ClusterServer> _logger;

    public ClusterServer(LiteServerOptions options, ILogger<ClusterServer> logger, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Login Server listening on port {Options.Port}.");
    }

    protected override void OnError(ClusterUser connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(ClusterServer).Name}.");
    }
}
