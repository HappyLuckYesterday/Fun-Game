using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots.Quests;

public class SetQuestSnapshot : FFSnapshot
{
    public SetQuestSnapshot(IPlayer player, IQuest quest)
        : base(SnapshotType.SETQUEST, player.Id)
    {
        quest.Serialize(this);
    }
}
