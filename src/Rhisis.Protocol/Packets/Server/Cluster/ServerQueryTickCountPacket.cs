namespace Rhisis.Protocol.Packets.Server.Cluster
{
    public class ServerQueryTickCountPacket : FFPacket
    {
        public ServerQueryTickCountPacket(uint time, long elapsedTime)
            : base(PacketType.QUERYTICKCOUNT)
        {
            WriteUInt32(time);
            WriteInt64(elapsedTime);
        }
    }
}
