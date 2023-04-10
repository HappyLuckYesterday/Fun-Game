using Rhisis.Protocol;
using Rhisis.WorldServer.Handlers;

namespace Rhisis.WorldServer;

public partial class SnapshotDispatcher
{
    static partial void OnBeforeExecute(FFUserConnection user, object handler)
    {
        if (handler is WorldPacketHandler worldHandler)
        {
            worldHandler.User = user as WorldUser;
        }
    }

    static partial void OnHandlerNotImplemented(FFUserConnection user, SnapshotType packetType)
    {
        user.SnapshotNotImplemented(packetType);
    }
}