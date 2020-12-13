using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Packets.Friends
{
    public class FriendNoInterceptPacket : FFPacket
    {
        public FriendNoInterceptPacket(int friendId, MessengerStatusType statusType)
            : base(PacketType.FRIENDNOINTERCEPT)
        {
            WriteUInt32((uint)friendId);
            WriteInt32((int)statusType);
        }
    }
}
