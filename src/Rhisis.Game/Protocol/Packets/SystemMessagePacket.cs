using Rhisis.Network;

namespace Rhisis.Game.Protocol.Packets
{
    public class SystemMessagePacket : FFPacket
    {
        public SystemMessagePacket(string message)
            : base(PacketType.SYSTEM)
        {
            Write(message);
        }
    }
}
