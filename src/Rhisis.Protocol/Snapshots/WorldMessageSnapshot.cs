using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class WorldMessageSnapshot : FFSnapshot
    {
        public WorldMessageSnapshot(IPlayer player, string message)
            : base(SnapshotType.WORLDMSG, player.Id)
        {
            Write(message);
        }
    }
}
