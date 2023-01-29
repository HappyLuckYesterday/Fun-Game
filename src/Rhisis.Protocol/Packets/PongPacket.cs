namespace Rhisis.Protocol.Packets;

public sealed class PongPacket : FFPacket
{
    public PongPacket(int time)
        : base(PacketType.PING)
    {
        WriteInt32(time);
    }
}