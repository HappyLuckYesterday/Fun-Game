using Microsoft.Extensions.Logging;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol;
using Rhisis.Protocol.Networking;
using System;
using System.Linq;

namespace Rhisis.LoginServer.Core.Handlers;

internal sealed class ClusterUpdateChannelHandler : IFFInterServerConnectionHandler<CoreCacheUser, ClusterUpdateChannelPacket>
{
    private readonly ILogger<ClusterUpdateChannelHandler> _logger;

    public ClusterUpdateChannelHandler(ILogger<ClusterUpdateChannelHandler> logger)
    {
        _logger = logger;
    }

    public void Execute(CoreCacheUser user, ClusterUpdateChannelPacket message)
    {
        if (user.IsAuthenticated)
        {
            WorldChannelInfo channel = user.Cluster.Channels.SingleOrDefault(x => x.Name == message.Channel.Name);

            if (channel is null)
            {
                throw new InvalidOperationException($"Channel with name '{message.Channel.Name}' not found in cluster '{user.Cluster.Name}'.");
            }

            channel.ConnectedUsers = message.Channel.ConnectedUsers;

            _logger.LogInformation($"World channel '{message.Channel.Name}' updated for '{user.Cluster.Name}'.");
        }
    }
}
