using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class ClearUseSkillSnapshot : FFSnapshot
{
    public ClearUseSkillSnapshot(Mover mover)
        : base(SnapshotType.CLEAR_USESKILL, mover.ObjectId)
    {
    }
}
