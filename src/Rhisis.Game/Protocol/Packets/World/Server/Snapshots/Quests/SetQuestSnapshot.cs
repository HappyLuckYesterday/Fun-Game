using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Quests;

public class SetQuestSnapshot : FFSnapshot
{
    public SetQuestSnapshot(Player player, Quest quest)
        : base(SnapshotType.SETQUEST, player.ObjectId)
    {
        quest.Serialize(this);
    }
}
