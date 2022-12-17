using Rhisis.Game.Common;

namespace Rhisis.Protocol.Packets.Server.World.Friends;

public class FriendNoInterceptPacket : FFPacket
{
    public FriendNoInterceptPacket(int friendId, MessengerStatusType statusType)
        : base(PacketType.FRIENDNOINTERCEPT)
    {
        WriteUInt32((uint)friendId);
        WriteInt32((int)statusType);
    }
}
