using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
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
