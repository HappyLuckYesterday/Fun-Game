using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Protocol.Snapshots.Quests;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Quests
{
    /// <summary>
    /// Handles all quest packets.
    /// </summary>
    [Handler]
    public class QuestCheckHandler
    {
        [HandlerAction(PacketType.QUEST_CHECK)]
        public void Execute(IPlayer player, QuestCheckPacket questCheckPacket)
        {
            if (questCheckPacket.QuestId <= 0)
            {
                throw new InvalidOperationException($"Invalid quest id: '{questCheckPacket.QuestId}'.");
            }

            IQuest quest = player.Quests.GetActiveQuest(questCheckPacket.QuestId);

            if (quest == null)
            {
                throw new InvalidOperationException($"Failed to find quest with id: '{questCheckPacket.QuestId}'.");
            }

            quest.IsChecked = !quest.IsChecked;

            using var questCheckedSnapshot = new QuestCheckedSnapshot(player, player.Quests.CheckedQuests);
            player.Send(questCheckedSnapshot);
        }
    }
}
