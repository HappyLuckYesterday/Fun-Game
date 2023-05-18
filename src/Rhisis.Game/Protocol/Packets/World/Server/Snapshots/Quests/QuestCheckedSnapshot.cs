using Rhisis.Game;
using Rhisis.Game.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Snapshots.Quests;

public class QuestCheckedSnapshot : FFSnapshot
{
    public QuestCheckedSnapshot(Player player, IEnumerable<Quest> checkedQuests)
        : base(SnapshotType.QUEST_CHECKED, player.ObjectId)
    {
        WriteByte((byte)checkedQuests.Count());

        foreach (Quest quest in checkedQuests)
        {
            WriteInt16((short)quest.Id);
        }
    }
}
