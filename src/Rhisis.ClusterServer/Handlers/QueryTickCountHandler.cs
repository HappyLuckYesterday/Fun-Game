using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.QUERYTICKCOUNT)]
internal class QueryTickCountHandler : ClusterHandlerBase, IPacketHandler
{
}