using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer;

public sealed class ClusterCacheClient : LiteClient
{
    private readonly ILogger<ClusterCacheClient> _logger;

    public ClusterCacheClient(LiteClientOptions options, ILogger<ClusterCacheClient> logger, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        return Task.CompletedTask;
    }
}