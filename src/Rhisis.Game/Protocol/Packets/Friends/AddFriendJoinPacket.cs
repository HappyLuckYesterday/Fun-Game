using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Packets.Friends
{
    public class AddFriendJoinPacket : FFPacket
    {
        public AddFriendJoinPacket(IContact contact)
            : base(PacketType.ADDFRIENDJOIN)
        {
            WriteUInt32((uint)contact.Id);
            WriteInt32((int)contact.Status);
            WriteInt32(contact.Status == MessengerStatusType.Offline ? 0 : 1);
        }
    }
}
