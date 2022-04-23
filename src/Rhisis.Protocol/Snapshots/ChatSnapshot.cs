using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class ChatSnapshot : FFSnapshot
    {
        public ChatSnapshot(IWorldObject worldObject, string text)
            : base(SnapshotType.CHAT, worldObject.Id)
        {
            WriteString(text);
        }
    }
}
