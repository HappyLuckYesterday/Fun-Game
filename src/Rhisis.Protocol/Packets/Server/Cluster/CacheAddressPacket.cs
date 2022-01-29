namespace Rhisis.Protocol.Packets.Server.Cluster
{
    public class CacheAddressPacket : FFPacket
    {
        public CacheAddressPacket(string address)
            : base(PacketType.CACHE_ADDR)
        {
            WriteString(address);
        }
    }
}
