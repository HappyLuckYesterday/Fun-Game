using Rhisis.Game;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Snapshots.Quests;
using System;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.QUEST_CHECK)]
internal sealed class QuestCheckHandler : WorldPacketHandler
{
    public void Execute(QuestCheckPacket packet)
    {
        if (packet.QuestId <= 0)
        {
            throw new InvalidOperationException($"Invalid quest id: '{packet.QuestId}'.");
        }

        Quest quest = Player.QuestDiary.GetActiveQuest(packet.QuestId) 
            ?? throw new InvalidOperationException($"Failed to find quest with id: '{packet.QuestId}'.");
        
        quest.IsChecked = !quest.IsChecked;

        using QuestCheckedSnapshot questCheckedSnapshot = new(Player, Player.QuestDiary.CheckedQuests);
        Player.Send(questCheckedSnapshot);
    }
}
