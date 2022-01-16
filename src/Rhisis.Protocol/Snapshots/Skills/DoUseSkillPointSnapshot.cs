using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Skills
{
    public class DoUseSkillPointSnapshot : FFSnapshot
    {
        public DoUseSkillPointSnapshot(IPlayer player)
            : base(SnapshotType.DOUSESKILLPOINT, player.Id)
        {
            player.SkillTree.Serialize(this);
            Write((int)player.SkillTree.SkillPoints);
        }
    }
}
