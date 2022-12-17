using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.ClusterServer.Core.Handlers;

[Handler]
public class PlayerDisconnectedFromChannelHandler
{
    private readonly IClusterServer _clusterServer;

    public PlayerDisconnectedFromChannelHandler(IClusterServer clusterServer)
    {
        _clusterServer = clusterServer;
    }

    [HandlerAction(CorePacketType.PlayerDisconnectedFromChannel)]
    public void OnExecute(ClusterCoreClient _, CorePacket packet)
    {
        int userId = packet.ReadInt32();
        IClusterUser clusterClient = _clusterServer.GetClientByUserId(userId);

        if (clusterClient is null)
        {
            throw new InvalidOperationException($"Cannot find cluster client with id: '{userId}'.");
        }

        clusterClient.Disconnect();
    }
}
