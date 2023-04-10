using Rhisis.Game.Protocol.Packets.Cluster.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.QUERYTICKCOUNT)]
internal class QueryTickCountHandler : ClusterHandlerBase
{
    public void Execute(QueryTickCountPacket packet)
    {
        User.SendQueryTickCount(packet.Time);
    }
}