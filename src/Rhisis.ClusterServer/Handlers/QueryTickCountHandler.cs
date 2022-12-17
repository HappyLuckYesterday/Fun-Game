using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.IO;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
using Rhisis.Protocol.Packets.Server.Cluster;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Handlers;

[Handler]
public class QueryTickCountHandler
{
    [HandlerAction(PacketType.QUERYTICKCOUNT)]
    public void OnQueryTickCount(IClusterUser user, QueryTickCountPacket packet)
    {
        using var queryTickCountPacket = new ServerQueryTickCountPacket(packet.Time, Time.GetElapsedTime());

        user.Send(queryTickCountPacket);
    }
}
