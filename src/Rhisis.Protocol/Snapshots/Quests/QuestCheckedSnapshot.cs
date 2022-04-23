using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Snapshots.Quests
{
    public class QuestCheckedSnapshot : FFSnapshot
    {
        public QuestCheckedSnapshot(IPlayer player, IEnumerable<IQuest> checkedQuests)
            : base(SnapshotType.QUEST_CHECKED, player.Id)
        {
            WriteByte((byte)checkedQuests.Count());

            foreach (IQuest quest in checkedQuests)
            {
                WriteInt16((short)quest.Id);
            }
        }
    }
}
