using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Skills
{
    public class InitSkillPointSnapshot : FFSnapshot
    {
        public InitSkillPointSnapshot(IPlayer player, ushort skillPoints)
            : base(SnapshotType.INITSKILLPOINT, player.Id)
        {
            WriteInt32((int)skillPoints);
        }
    }
}
