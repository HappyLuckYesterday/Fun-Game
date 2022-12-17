namespace Rhisis.Protocol.Packets.Server.World;

public class SystemMessagePacket : FFPacket
{
    public SystemMessagePacket(string message)
        : base(PacketType.SYSTEM)
    {
        WriteString(message);
    }
}
