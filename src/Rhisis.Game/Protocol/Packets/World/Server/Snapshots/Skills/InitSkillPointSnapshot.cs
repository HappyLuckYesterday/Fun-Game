using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;

public class InitSkillPointSnapshot : FFSnapshot
{
    public InitSkillPointSnapshot(Player player, ushort skillPoints)
        : base(SnapshotType.INITSKILLPOINT, player.ObjectId)
    {
        WriteInt32(skillPoints);
    }
}
