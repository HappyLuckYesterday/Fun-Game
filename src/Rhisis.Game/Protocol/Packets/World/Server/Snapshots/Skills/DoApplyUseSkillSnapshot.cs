using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class DoApplyUseSkillSnapshot : FFSnapshot
{
    public DoApplyUseSkillSnapshot(Mover mover, uint targetId, int skillId, int skillLevel)
        : base(SnapshotType.DOAPPLYUSESKILL, mover.ObjectId)
    {
        WriteUInt32(targetId);
        WriteInt32(skillId);
        WriteInt32(skillLevel);
    }
}
