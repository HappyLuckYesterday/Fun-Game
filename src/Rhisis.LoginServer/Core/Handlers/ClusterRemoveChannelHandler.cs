using Microsoft.Extensions.Logging;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;

namespace Rhisis.LoginServer.Core.Handlers;

internal sealed class ClusterRemoveChannelHandler : IFFInterServerConnectionHandler<CoreCacheUser, ClusterRemoveChannelPacket>
{
    private readonly ILogger<ClusterRemoveChannelHandler> _logger;
    private readonly IClusterCache _clusterCache;

    public ClusterRemoveChannelHandler(ILogger<ClusterRemoveChannelHandler> logger, IClusterCache clusterCache)
    {
        _logger = logger;
        _clusterCache = clusterCache;
    }

    public void Execute(CoreCacheUser user, ClusterRemoveChannelPacket message)
    {
        if (user.IsAuthenticated)
        {
            _clusterCache.RemoveCluster(message.ChannelName);

            _logger.LogInformation($"World channel '{message.ChannelName}' removed from '{user.Cluster.Name}'.");
        }
    }
}