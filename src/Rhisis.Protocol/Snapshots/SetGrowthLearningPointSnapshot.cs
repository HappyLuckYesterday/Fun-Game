using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class SetGrowthLearningPointSnapshot : FFSnapshot
{
    public SetGrowthLearningPointSnapshot(IPlayer player)
        : base(SnapshotType.SET_GROWTH_LEARNING_POINT, player.Id)
    {
        WriteInt64(player.Statistics.AvailablePoints);
        WriteInt64(0);
    }
}
