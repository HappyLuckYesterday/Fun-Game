using Rhisis.Abstractions.Features;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Packets.Server.World.Friends;

public class GetFriendStatePacket : FFPacket
{
    public GetFriendStatePacket(IEnumerable<IContact> unblockedFirends, IEnumerable<IContact> blockedFriends)
        : base(PacketType.GETFRIENDSTATE)
    {
        WriteInt32(unblockedFirends.Count());
        WriteInt32(blockedFriends.Count());

        foreach (IContact contact in unblockedFirends)
        {
            WriteInt32(contact.Id);
            WriteInt32((int)contact.Status);
            WriteInt32(contact.IsOnline ? contact.Channel : 100);
        }

        foreach (IContact contact in blockedFriends)
        {
            WriteInt32(contact.Id);
            WriteInt32((int)contact.Status);
            WriteInt32(contact.IsOnline ? contact.Channel : 100);
        }
    }
}
