using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SetExperienceSnapshot : FFSnapshot
{
    public SetExperienceSnapshot(Player player)
        : base(SnapshotType.SETEXPERIENCE, player.ObjectId)
    {
        WriteInt64(player.Experience.Amount);
        WriteInt16((short)player.Level);
        WriteInt32(0);
        WriteInt32(player.SkillPoints);
        WriteInt64(long.MaxValue); // death exp
        WriteInt16((short)player.DeathLevel);
    }
}
