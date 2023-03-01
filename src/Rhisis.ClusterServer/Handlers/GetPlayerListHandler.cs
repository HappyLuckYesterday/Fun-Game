using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.GETPLAYERLIST)]
internal class GetPlayerListHandler : ClusterHandlerBase, IPacketHandler
{
    public void Execute(GetPlayerListPacket packet)
    {
        // TODO
    }
}
