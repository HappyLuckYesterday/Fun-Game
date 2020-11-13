using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Skills
{
    public class SetSkillStateSnapshot : FFSnapshot
    {
        public SetSkillStateSnapshot(IMover mover, ISkill skill, int time)
            : base(SnapshotType.SETSKILLSTATE, mover.Id)
        {
            Write(mover.Id);
            Write((short)BuffType.Skill);
            Write((short)skill.Id);
            Write(skill.Level);
            Write(time);
        }
    }
}
