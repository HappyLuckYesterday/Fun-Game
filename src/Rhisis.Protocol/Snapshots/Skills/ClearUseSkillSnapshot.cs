using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Skills;

public class ClearUseSkillSnapshot : FFSnapshot
{
    public ClearUseSkillSnapshot(IMover mover)
        : base(SnapshotType.CLEAR_USESKILL, mover.Id)
    {
    }
}
