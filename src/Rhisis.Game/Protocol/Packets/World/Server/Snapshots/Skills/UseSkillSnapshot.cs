using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class UseSkillSnapshot : FFSnapshot
{
    public UseSkillSnapshot(Mover mover, Mover target, Skill skill, int castingTime, SkillUseType skillUseType)
         : base(SnapshotType.USESKILL, mover.ObjectId)
    {
        WriteInt32(skill.Id);
        WriteInt32(skill.Level);
        WriteUInt32(target.ObjectId);
        WriteInt32((int)skillUseType);
        WriteInt32(castingTime);
    }
}
