using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using System.Linq;

namespace Rhisis.Game.Protocol.Snapshots.Skills
{
    public class SetJobSkill : FFSnapshot
    {
        public SetJobSkill(IPlayer player)
            : base(SnapshotType.SET_JOB_SKILL, player.Id)
        {
            WriteInt32((int)player.Job.Id);
            player.SkillTree.Serialize(this);
            Write(Enumerable.Repeat((byte)0, 128).ToArray(), 0, 128);
        }
    }
}
