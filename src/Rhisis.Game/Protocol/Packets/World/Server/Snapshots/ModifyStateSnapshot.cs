using Rhisis.Game.Entities;

namespace Rhisis.Protocol.Snapshots;

public class ModifyStateSnapshot : FFSnapshot
{
    public ModifyStateSnapshot(Player player)
        : base(SnapshotType.SETSTATE, player.ObjectId)
    {
        WriteInt32(player.Statistics.Strength);
        WriteInt32(player.Statistics.Stamina);
        WriteInt32(player.Statistics.Dexterity);
        WriteInt32(player.Statistics.Intelligence);
        WriteInt32(0);
    }
}