using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Login.Clients;

namespace Rhisis.LoginServer.Handlers;

[PacketHandler(PacketType.ERROR)]
internal sealed class ErrorHandler : LoginPacketHandler<LoginErrorPacket>
{
    public override void Execute(LoginErrorPacket message)
    {
        if (Server.IsUserConnected(User.Username))
        {
            User.Disconnect();
        }
    }
}
