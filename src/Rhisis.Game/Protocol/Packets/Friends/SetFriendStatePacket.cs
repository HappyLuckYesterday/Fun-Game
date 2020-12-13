using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Packets.Friends
{
    public class SetFriendStatePacket : FFPacket
    {
        public SetFriendStatePacket(int playerId, MessengerStatusType messengerStatus)
            : base(PacketType.SETFRIENDSTATE)
        {
            WriteInt32(playerId);
            WriteInt32((int)messengerStatus);
        }
    }
}
