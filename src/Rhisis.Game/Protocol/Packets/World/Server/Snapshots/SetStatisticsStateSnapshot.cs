using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SetStatisticsStateSnapshot : FFSnapshot
{
    public SetStatisticsStateSnapshot(Player player)
        : base(SnapshotType.SETSTATE, player.ObjectId)
    {
        WriteInt32(player.Statistics.Strength);
        WriteInt32(player.Statistics.Stamina);
        WriteInt32(player.Statistics.Dexterity);
        WriteInt32(player.Statistics.Intelligence);
        WriteInt32(0);
        WriteUInt32((uint)player.AvailablePoints);
    }
}
