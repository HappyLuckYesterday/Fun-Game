using Rhisis.Game.Protocol.Packets;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.LoginServer.Handlers;

[PacketHandler(PacketType.PING)]
public sealed class PingHandler : LoginPacketHandler
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
