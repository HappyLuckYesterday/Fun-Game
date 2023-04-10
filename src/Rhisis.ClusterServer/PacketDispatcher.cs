using Rhisis.ClusterServer.Handlers;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer;

public partial class PacketDispatcher
{
    static partial void OnBeforeExecute(FFUserConnection user, object handler)
    {
        if (handler is ClusterHandlerBase packetHandler)
        {
            packetHandler.User = user as ClusterUser;
        }
    }

    static partial void OnHandlerNotImplemented(FFUserConnection user, PacketType packetType)
    {
        user.PacketHandlerNotImplemented(packetType);
    }
}
