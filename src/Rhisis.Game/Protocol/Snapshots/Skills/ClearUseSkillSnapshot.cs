using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Skills
{
    public class ClearUseSkillSnapshot : FFSnapshot
    {
        public ClearUseSkillSnapshot(IMover mover)
            : base(SnapshotType.CLEAR_USESKILL, mover.Id)
        {
        }
    }
}
