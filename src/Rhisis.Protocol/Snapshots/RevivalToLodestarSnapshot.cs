using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class RevivalToLodestarSnapshot : FFSnapshot
{
    public RevivalToLodestarSnapshot(IPlayer player)
        : base(SnapshotType.REVIVAL_TO_LODESTAR, player.Id)
    {
    }
}
