using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class SetExperienceSnapshot : FFSnapshot
{
    public SetExperienceSnapshot(IPlayer player)
        : base(SnapshotType.SETEXPERIENCE, player.Id)
    {
        WriteInt64(player.Experience.Amount);
        WriteInt16((short)player.Level);
        WriteInt32(0);
        WriteInt32(player.SkillTree.SkillPoints);
        WriteInt64(long.MaxValue); // death exp
        WriteInt16((short)player.DeathLevel);
    }
}
