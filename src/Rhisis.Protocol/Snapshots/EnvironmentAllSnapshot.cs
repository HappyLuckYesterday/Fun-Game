using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class EnvironmentAllSnapshot : FFSnapshot
{
    public EnvironmentAllSnapshot(IPlayer player, SeasonType season)
        : base(SnapshotType.ENVIRONMENTALL, player.Id)
    {
        WriteInt32((int)season);
    }
}
