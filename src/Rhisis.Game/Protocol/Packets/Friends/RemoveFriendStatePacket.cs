using Rhisis.Network;

namespace Rhisis.Game.Protocol.Packets.Friends
{
    public class RemoveFriendStatePacket : FFPacket
    {
        public RemoveFriendStatePacket(int removedFriendId)
            : base(PacketType.REMOVEFRIENDSTATE)
        {
            WriteInt32(removedFriendId);
        }
    }
}
