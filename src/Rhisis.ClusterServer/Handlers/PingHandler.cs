using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.PING)]
internal class PingHandler : ClusterHandlerBase, IPacketHandler
{
    public void Execute(PingPacket packet)
    {
        if (!packet.IsTimeOut)
        {
            User.SendPong(packet.Time);
        }
    }
}
