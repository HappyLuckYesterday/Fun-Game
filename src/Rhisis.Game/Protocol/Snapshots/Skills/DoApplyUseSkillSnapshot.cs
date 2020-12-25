using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Skills
{
    public class DoApplyUseSkillSnapshot : FFSnapshot
    {
        public DoApplyUseSkillSnapshot(IMover mover, uint targetId, int skillId, int skillLevel)
            : base(SnapshotType.DOAPPLYUSESKILL, mover.Id)
        {
            WriteUInt32(targetId);
            WriteInt32(skillId);
            WriteInt32(skillLevel);
        }
    }
}
