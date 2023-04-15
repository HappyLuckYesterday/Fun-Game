using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class ChatSnapshot : FFSnapshot
{
    public ChatSnapshot(WorldObject worldObject, string text)
        : base(SnapshotType.CHAT, worldObject.ObjectId)
    {
        WriteString(text);
    }
}
