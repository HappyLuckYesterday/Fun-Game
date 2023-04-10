using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System.Linq;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class SetJobSkill : FFSnapshot
{
    public SetJobSkill(Player player)
        : base(SnapshotType.SET_JOB_SKILL, player.ObjectId)
    {
        WriteInt32((int)player.Job.Id);
        player.Skills.Serialize(this);
        Write(Enumerable.Repeat((byte)0, 128).ToArray(), 0, 128);
    }
}
