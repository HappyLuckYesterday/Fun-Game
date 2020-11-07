using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Skills
{
    public class InitSkillPointSnapshot : FFSnapshot
    {
        public InitSkillPointSnapshot(IPlayer player, ushort skillPoints)
            : base(SnapshotType.INITSKILLPOINT, player.Id)
        {
            Write((int)skillPoints);
        }
    }
}
