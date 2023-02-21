using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Login.Clients;

namespace Rhisis.LoginServer.Handlers;

[PacketHandler(PacketType.ERROR)]
public sealed class ErrorHandler : LoginPacketHandler, IPacketHandler
{
    private readonly LoginServer _server;

    public ErrorHandler(LoginServer _server)
    {
        this._server = _server;
    }

    public void Execute(LoginErrorPacket message)
    {
        if (_server.IsUserConnected(User.Username))
        {
            User.Disconnect();
        }
    }
}
