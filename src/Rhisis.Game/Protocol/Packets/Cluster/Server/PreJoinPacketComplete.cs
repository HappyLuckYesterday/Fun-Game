using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.Cluster.Server;

public class PreJoinPacketComplete : FFPacket
{
    public PreJoinPacketComplete()
        : base(PacketType.PRE_JOIN)
    {
    }
}
