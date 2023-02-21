using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets;

namespace Rhisis.LoginServer.Handlers;

[PacketHandler(PacketType.PING)]
public sealed class PingHandler : LoginPacketHandler, IPacketHandler
{
    public void Execute(PingPacket message)
    {
        if (!message.IsTimeOut)
        {
            using var pongPacket = new PongPacket(message.Time);
            User.Send(pongPacket);
        }
    }
}
