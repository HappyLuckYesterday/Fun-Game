using Rhisis.LoginServer.Handlers;
using Rhisis.Protocol;

namespace Rhisis.LoginServer;

public partial class PacketDispatcher
{
    static partial void OnBeforeExecute(FFUserConnection user, object handler)
    {
        if (handler is LoginPacketHandler loginPacketHandler)
        {
            loginPacketHandler.User = user as LoginUser;
        }
    }

    static partial void OnHandlerNotImplemented(FFUserConnection user, PacketType packetType)
    {
        user.PacketHandlerNotImplemented(packetType);
    }
}