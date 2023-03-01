using Rhisis.ClusterServer.Handlers;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer;

public partial class PacketDispatcher
{
    static partial void OnBeforeExecute(FFUserConnection user, IPacketHandler handler)
    {
        if (handler is ClusterHandlerBase packetHandler)
        {
            packetHandler.User = user as ClusterUser;
        }
    }
}
