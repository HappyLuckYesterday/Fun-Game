namespace Rhisis.Protocol.Packets.Server
{
    public class WelcomePacket : FFPacket
    {
        public WelcomePacket(uint sessionId)
             : base(PacketType.WELCOME)
        {
            WriteUInt32(sessionId);
        }
    }
}
