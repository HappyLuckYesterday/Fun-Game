using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SetGrowthLearningPointSnapshot : FFSnapshot
{
    public SetGrowthLearningPointSnapshot(Player player)
        : base(SnapshotType.SET_GROWTH_LEARNING_POINT, player.ObjectId)
    {
        WriteInt64(player.AvailablePoints);
        WriteInt64(0);
    }
}
