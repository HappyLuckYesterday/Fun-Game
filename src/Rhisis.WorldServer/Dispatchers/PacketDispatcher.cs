using Rhisis.Protocol;
using Rhisis.WorldServer.Handlers;

namespace Rhisis.WorldServer;

public partial class PacketDispatcher
{
    static partial void OnBeforeExecute(FFUserConnection user, object handler)
    {
        if (handler is WorldPacketHandler worldHandler)
        {
            worldHandler.User = user as WorldUser;
        }
    }

    static partial void OnHandlerNotImplemented(FFUserConnection user, PacketType packetType)
    {
        user.PacketHandlerNotImplemented(packetType);
    }
}
