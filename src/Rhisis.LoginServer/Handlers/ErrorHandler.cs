using Rhisis.Game.Protocol.Packets.Login.Clients;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.LoginServer.Handlers;

[PacketHandler(PacketType.ERROR)]
public sealed class ErrorHandler : LoginPacketHandler
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
