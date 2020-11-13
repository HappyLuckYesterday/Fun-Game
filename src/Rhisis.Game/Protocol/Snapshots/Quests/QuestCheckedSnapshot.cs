using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Protocol.Snapshots.Quests
{
    public class QuestCheckedSnapshot : FFSnapshot
    {
        public QuestCheckedSnapshot(IPlayer player, IEnumerable<IQuest> checkedQuests)
            : base(SnapshotType.QUEST_CHECKED, player.Id)
        {
            Write((byte)checkedQuests.Count());

            foreach (IQuest quest in checkedQuests)
            {
                Write((short)quest.Id);
            }
        }
    }
}
