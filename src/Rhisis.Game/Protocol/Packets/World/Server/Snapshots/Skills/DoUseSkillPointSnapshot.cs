using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class DoUseSkillPointSnapshot : FFSnapshot
{
    public DoUseSkillPointSnapshot(Player player)
        : base(SnapshotType.DOUSESKILLPOINT, player.ObjectId)
    {
        player.Skills.Serialize(this);
        WriteInt32(player.SkillPoints);
    }
}
