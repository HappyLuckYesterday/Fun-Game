using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.PING)]
internal class PingHandler : ClusterHandlerBase, IPacketHandler
{
}
