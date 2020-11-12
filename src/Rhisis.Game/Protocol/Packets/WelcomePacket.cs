using Rhisis.Network;

namespace Rhisis.Game.Protocol.Packets
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
