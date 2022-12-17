namespace Rhisis.Protocol.Packets.Server.World.Friends;

public class RemoveFriendStatePacket : FFPacket
{
    public RemoveFriendStatePacket(int removedFriendId)
        : base(PacketType.REMOVEFRIENDSTATE)
    {
        WriteInt32(removedFriendId);
    }
}
