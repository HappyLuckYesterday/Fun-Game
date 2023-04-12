using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class EnvironmentAllSnapshot : FFSnapshot
{
    public EnvironmentAllSnapshot(Player player, SeasonType season)
        : base(SnapshotType.ENVIRONMENTALL, player.ObjectId)
    {
        WriteInt32((int)season);
    }
}
