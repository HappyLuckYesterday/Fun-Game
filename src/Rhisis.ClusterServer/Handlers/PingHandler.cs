using Rhisis.Game.Protocol.Packets;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.PING)]
internal class PingHandler : ClusterHandlerBase
{
    public void Execute(PingPacket packet)
    {
        if (!packet.IsTimeOut)
        {
            User.SendPong(packet.Time);
        }
    }
}
