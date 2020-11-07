using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Skills
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
