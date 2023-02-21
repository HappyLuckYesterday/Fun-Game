using Rhisis.LoginServer.Handlers;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.LoginServer;

public partial class PacketDispatcher
{
    static partial void OnBeforeExecute(FFUserConnection user, IPacketHandler handler)
    {
        if (handler is LoginPacketHandler loginPacketHandler)
        {
            loginPacketHandler.User = user as LoginUser;
        }
    }
}