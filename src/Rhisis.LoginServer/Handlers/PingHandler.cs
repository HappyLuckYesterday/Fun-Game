using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client;
using Rhisis.Protocol.Packets.Server;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.LoginServer.Handlers;

[Handler]
public class PingHandler
{
    /// <summary>
    /// Handles the PING packet.
    /// </summary>
    /// <param name="user">Client.</param>
    /// <param name="packet">Ping packet.</param>
    [HandlerAction(PacketType.PING)]
    public void OnPing(ILoginUser user, PingPacket packet)
    {
        if (!packet.IsTimeOut)
        {
            using var pongPacket = new PongPacket(packet.Time);
            user.Send(pongPacket);
        }
    }
}