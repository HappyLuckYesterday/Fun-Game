using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class SetStatisticsStateSnapshot : FFSnapshot
{
    public SetStatisticsStateSnapshot(IPlayer player)
        : base(SnapshotType.SETSTATE, player.Id)
    {
        WriteInt32(player.Statistics.Strength);
        WriteInt32(player.Statistics.Stamina);
        WriteInt32(player.Statistics.Dexterity);
        WriteInt32(player.Statistics.Intelligence);
        WriteInt32(0);
        WriteUInt32(player.Statistics.AvailablePoints);
    }
}
