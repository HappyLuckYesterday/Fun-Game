using Rhisis.Game.Common;

namespace Rhisis.Protocol.Packets.Server.World.Friends
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
