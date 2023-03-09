using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;
using System;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.GETPLAYERLIST)]
internal class GetPlayerListHandler : ClusterHandlerBase, IPacketHandler
{
    private readonly ILogger<GetPlayerListHandler> _logger;

    public GetPlayerListHandler(ILogger<GetPlayerListHandler> logger)
    {
        _logger = logger;
    }

    public void Execute(GetPlayerListPacket packet)
    {
        //if (User.SessionId != packet.AuthenticationKey)
        //{
        //    throw new InvalidOperationException("Invalid user authentication key.");
        //}

        _logger.LogInformation($"Get player list handler.");   
    }
}
