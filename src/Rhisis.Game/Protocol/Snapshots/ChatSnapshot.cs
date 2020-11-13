using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
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
