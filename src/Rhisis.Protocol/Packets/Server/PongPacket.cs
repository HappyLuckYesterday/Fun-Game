namespace Rhisis.Protocol.Packets.Server;

public class PongPacket : FFPacket
{
    public PongPacket(int time)
        : base(PacketType.PING)
    {
        WriteInt32(time);
    }
}
