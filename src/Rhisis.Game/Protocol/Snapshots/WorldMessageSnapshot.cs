using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
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
