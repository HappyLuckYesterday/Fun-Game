using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Core;

namespace Rhisis.LoginServer.Caching.Handlers;

[CoreHandler(CorePacketType.UpdateClusterInfo)]
internal class ClusterInfoUpdateHandler : ClusterCacheHandler
{
    public void Execute(ClusterInfoUpdatePacket message)
    {

    }
}
