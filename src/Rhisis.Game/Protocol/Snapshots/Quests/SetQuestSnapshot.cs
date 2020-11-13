using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots.Quests
{
    public class SetQuestSnapshot : FFSnapshot
    {
        public SetQuestSnapshot(IPlayer player, IQuest quest)
            : base(SnapshotType.SETQUEST, player.Id)
        {
            quest.Serialize(this);
        }
    }
}
