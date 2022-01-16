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
            Write(skill.Id);
            Write(skill.Level);
            Write(target.Id);
            Write((int)skillUseType);
            Write(castingTime);
        }
    }
}
