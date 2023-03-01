using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.DEL_PLAYER)]
internal class DeletePlayerHandler : ClusterHandlerBase, IPacketHandler
{
}
