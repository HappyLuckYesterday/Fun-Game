using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Skills
{
    public class SetSkillStateSnapshot : FFSnapshot
    {
        public SetSkillStateSnapshot(IMover mover, int skillId, int skillLevel, int time)
            : base(SnapshotType.SETSKILLSTATE, mover.Id)
        {
            WriteUInt32(mover.Id);
            WriteInt16((short)BuffType.Skill);
            WriteInt16((short)skillId);
            WriteInt32(skillLevel);
            WriteInt32(time);
        }
    }
}
