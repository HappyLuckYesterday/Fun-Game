using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Core;

namespace Rhisis.LoginServer.Core.Handlers;

[CoreHandler(CorePacketType.UpdateClusterInfo)]
internal class ClusterInfoUpdateHandler : CoreCacheHandler
{
    private readonly ILogger<ClusterInfoUpdateHandler> _logger;
    private readonly IClusterCache _clusterCache;

    public ClusterInfoUpdateHandler(ILogger<ClusterInfoUpdateHandler> logger, IClusterCache clusterCache)
    {
        _logger = logger;
        _clusterCache = clusterCache;
    }

    public void Execute(ClusterInfoUpdatePacket message)
    {
        if (User.Cluster is null && _clusterCache.HasCluster(message.Cluster.Name))
        {
            _logger.LogWarning($"A cluster with name '{message.Cluster.Name}' is already connected.");

            User.Dispose();
        }

        User.Cluster = message.Cluster;
    }
}
