using Microsoft.Extensions.Logging;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol;
using Rhisis.Protocol.Networking;
using System;
using System.Linq;

namespace Rhisis.LoginServer.Core.Handlers;

internal sealed class ClusterAddChannelHandler : IFFInterServerConnectionHandler<CoreCacheUser, ClusterAddChannelPacket>
{
    private readonly ILogger<ClusterAddChannelHandler> _logger;

    public ClusterAddChannelHandler(ILogger<ClusterAddChannelHandler> logger)
    {
        _logger = logger;
    }

    public void Execute(CoreCacheUser user, ClusterAddChannelPacket message)
    {
        if (user.IsAuthenticated)
        {
            if (user.Cluster.Channels.Any(x => x.Name == message.Channel.Name))
            {
                throw new InvalidOperationException($"Failed to add channel '{message.Channel.Name}' to cluster '{user.Cluster.Name}' because a channel with the same name already exists.");
            }

            user.Cluster.Channels.Add(new WorldChannelInfo
            {
                Id = message.Channel.Id,
                Ip = message.Channel.Ip,
                Port = message.Channel.Port,
                Name = message.Channel.Name,
                IsEnabled = message.Channel.IsEnabled,
                MaximumUsers = message.Channel.MaximumUsers,
                ConnectedUsers = message.Channel.ConnectedUsers
            });

            _logger.LogInformation($"World channel '{message.Channel.Name}' added to '{user.Cluster.Name}'.");
        }
    }
}
