using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots.Skills
{
    public class UseSkillSnapshot : FFSnapshot
    {
        public UseSkillSnapshot(IMover mover, IMover target, ISkill skill, int castingTime, SkillUseType skillUseType)
             : base(SnapshotType.USESKILL, mover.Id)
        {
            WriteInt32(skill.Id);
            WriteInt32(skill.Level);
            WriteUInt32(target.Id);
            WriteInt32((int)skillUseType);
            WriteInt32(castingTime);
        }
    }
}
