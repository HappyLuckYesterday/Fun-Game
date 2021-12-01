using Rhisis.Game.Abstractions.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class ChatSnapshot : FFSnapshot
    {
        public ChatSnapshot(IWorldObject worldObject, string text)
            : base(SnapshotType.CHAT, worldObject.Id)
        {
            Write(text);
        }
    }
}
