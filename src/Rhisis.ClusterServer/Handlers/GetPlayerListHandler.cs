using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.GETPLAYERLIST)]
internal class GetPlayerListHandler : IPacketHandler
{
    public void Execute(GetPlayerListPacket packet)
    {
        // TODO
    }
}
