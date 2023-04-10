using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.Cluster.Server;

public class CacheAddressPacket : FFPacket
{
    public CacheAddressPacket(string address)
        : base(PacketType.CACHE_ADDR)
    {
        WriteString(address);
    }
}
