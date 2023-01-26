namespace Rhisis.Protocol.Packets;

public class ErrorPacket : FFPacket
{
    public ErrorPacket(ErrorType error)
        : base(PacketType.ERROR)
    {
        WriteInt32((int)error);
    }
}
