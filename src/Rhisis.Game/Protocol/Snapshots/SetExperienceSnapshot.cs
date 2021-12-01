using Rhisis.Game.Abstractions.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class SetExperienceSnapshot : FFSnapshot
    {
        public SetExperienceSnapshot(IPlayer player)
            : base(SnapshotType.SETEXPERIENCE, player.Id)
        {
            Write(player.Experience.Amount);
            Write((short)player.Level);
            Write(0);
            Write((int)player.SkillTree.SkillPoints);
            Write(long.MaxValue); // death exp
            Write((short)player.DeathLevel);
        }
    }
}
