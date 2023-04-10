using Rhisis.Game;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class SetSkillStateSnapshot : FFSnapshot
{
    public SetSkillStateSnapshot(Mover mover, int skillId, int skillLevel, int time)
        : base(SnapshotType.SETSKILLSTATE, mover.ObjectId)
    {
        WriteUInt32(mover.ObjectId);
        WriteInt16((short)BuffType.Skill);
        WriteInt16((short)skillId);
        WriteInt32(skillLevel);
        WriteInt32(time);
    }
}
