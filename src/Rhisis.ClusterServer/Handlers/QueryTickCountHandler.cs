using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.QUERYTICKCOUNT)]
internal class QueryTickCountHandler : ClusterHandlerBase, IPacketHandler
{
    public void Execute(QueryTickCountPacket packet)
    {
        User.SendQueryTickCount(packet.Time);
    }
}