using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
{
    public class RevivalToLodestarSnapshot : FFSnapshot
    {
        public RevivalToLodestarSnapshot(IPlayer player)
            : base(SnapshotType.REVIVAL_TO_LODESTAR, player.Id)
        {
        }
    }
}
