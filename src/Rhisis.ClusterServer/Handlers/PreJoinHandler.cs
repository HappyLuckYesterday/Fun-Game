using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.PRE_JOIN)]
internal class PreJoinHandler : ClusterHandlerBase, IPacketHandler
{
}
