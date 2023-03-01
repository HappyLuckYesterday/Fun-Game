using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.CREATE_PLAYER)]
internal class CreatePlayerHandler : ClusterHandlerBase, IPacketHandler
{
}
