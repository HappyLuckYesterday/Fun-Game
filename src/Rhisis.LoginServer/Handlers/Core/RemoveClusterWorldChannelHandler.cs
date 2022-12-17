using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.LoginServer.Core.Handlers;

[Handler]
public class RemoveClusterWorldChannelHandler
{
    private readonly ILogger<RemoveClusterWorldChannelHandler> _logger;

    public RemoveClusterWorldChannelHandler(ILogger<RemoveClusterWorldChannelHandler> logger)
    {
        _logger = logger;
    }

    [HandlerAction(CorePacketType.RemoveClusterWorldChannel)]
    public void OnExecute(CoreUser user, CorePacket packet)
    {
        int channelId = packet.ReadByte();

        WorldChannel channel = user.Cluster.Channels.SingleOrDefault(x => x.Id == channelId);

        if (channel is not null)
        {
            _logger.LogTrace($"Removing channel '{channel.Name}' from cluster '{user.Cluster.Name}'.");
            user.Cluster.Channels.Remove(channel);
        }
    }
}
