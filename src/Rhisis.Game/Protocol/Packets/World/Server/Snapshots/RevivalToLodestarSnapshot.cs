using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class RevivalToLodestarSnapshot : FFSnapshot
{
    public RevivalToLodestarSnapshot(Player player)
        : base(SnapshotType.REVIVAL_TO_LODESTAR, player.ObjectId)
    {
    }
}
