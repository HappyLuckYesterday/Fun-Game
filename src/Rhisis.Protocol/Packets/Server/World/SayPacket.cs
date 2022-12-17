namespace Rhisis.Protocol.Packets.Server.World;

public class SayPacket : FFPacket
{
    public SayPacket(int fromId, string fromName, int destinationId, string destinationName, string message)
        : base(PacketType.SAY)
    {
        WriteString(fromName);
        WriteString(destinationName);
        WriteString(message);
        WriteUInt32((uint)fromId);
        WriteUInt32((uint)destinationId);
        WriteInt32(0); // search
    }
}
