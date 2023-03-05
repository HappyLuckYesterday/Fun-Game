using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;
using System;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.GETPLAYERLIST)]
internal class GetPlayerListHandler : ClusterHandlerBase, IPacketHandler
{
    public void Execute(GetPlayerListPacket packet)
    {
        if (User.SessionId != packet.AuthenticationKey)
        {
            throw new InvalidOperationException("Invalid user authentication key.");
        }


    }
}
