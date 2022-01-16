using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class SetGrowthLearningPointSnapshot : FFSnapshot
    {
        public SetGrowthLearningPointSnapshot(IPlayer player)
            : base(SnapshotType.SET_GROWTH_LEARNING_POINT, player.Id)
        {
            Write((long)player.Statistics.AvailablePoints);
            Write((long)0);
        }
    }
}
