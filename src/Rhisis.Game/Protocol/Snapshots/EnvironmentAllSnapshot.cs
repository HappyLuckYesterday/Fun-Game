using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class EnvironmentAllSnapshot : FFSnapshot
    {
        public EnvironmentAllSnapshot(IPlayer player, SeasonType season)
            : base(SnapshotType.ENVIRONMENTALL, player.Id)
        {
            Write((int)season);
        }
    }
}
