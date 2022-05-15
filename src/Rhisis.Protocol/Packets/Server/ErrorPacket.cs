namespace Rhisis.Protocol.Packets.Server
{
    public class ErrorPacket : FFPacket
    {
        public ErrorPacket(ErrorType error)
            : base(PacketType.ERROR)
        {
            WriteInt32((int)error);
        }
    }
}
