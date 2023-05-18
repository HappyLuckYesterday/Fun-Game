using Rhisis.Core.Extensions;
using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Game.Resources.Properties.Dialogs;
using Rhisis.Game.Resources.Properties.Quests;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Npcs;

[PacketHandler(PacketType.SCRIPTDLG)]
internal sealed class ScriptDialogHandler : WorldPacketHandler
{
    public void Execute(DialogPacket packet)
    {
        if (packet.ObjectId <= 0)
        {
            throw new ArgumentException("Invalid object id.");
        }

        Npc npc = Player.VisibleObjects.OfType<Npc>().SingleOrDefault(x => x.ObjectId == packet.ObjectId)
            ?? throw new ArgumentException($"Cannot find NPC with object id: {packet.ObjectId}");
        Player.StopMoving();

        string dialogKey = packet.DialogKey;
        int questId = packet.QuestId;

        if (string.IsNullOrEmpty(dialogKey))
        {
            if (npc.HasQuests)
            {
                if (!npc.SuggestFinalizeQuest(Player))
                {
                    if (npc.SuggestAvailableQuest(Player))
                    {
                        return;
                    }
                }
            }

            if (npc.HasDialog)
            {
                npc.ShowDialog(Player, npc.Properties.Dialog.IntroText);
            }
        }
        else
        {
            if (questId == 0)
            {
                if (!npc.HasDialog)
                {
                    if (dialogKey == DialogConstants.Bye)
                    {
                        npc.CloseDialog(Player);
                    }
                }
                else
                {
                    if (dialogKey == DialogConstants.Bye)
                    {
                        npc.CloseDialog(Player);
                        npc.Speak(npc.Properties.Dialog.ByeText, Player);
                    }
                    else
                    {
                        DialogLink dialogLink = npc.Properties.Dialog.Links?.FirstOrDefault(x => x.Id == dialogKey)
                            ?? throw new InvalidOperationException($"Cannot find dialog key: '{dialogKey}' for NPC '{npc.Name}'.");
                        
                        npc.ShowDialog(Player, dialogLink.Texts);
                    }
                }
            }
            else
            {
                QuestProperties questProperties = npc.Quests.FirstOrDefault(x => x.Id == questId);

                if (questProperties is null)
                {
                    // If not, check if the npc is the end character of player's active quest
                    questProperties = Player.QuestDiary.ActiveQuests
                        .Where(q => q.Id == questId && q.Properties.EndCharacter.Equals(npc.Name, StringComparison.OrdinalIgnoreCase))
                        .Select(q => q.Properties)
                        .FirstOrDefault();

                    if (questProperties is null)
                    {
                        throw new InvalidOperationException($"Cannot find quest with id '{questId}' at npc '{npc}' for player '{Player.Name}'.");
                    }
                }

                HandleQuestState(Player, npc, questProperties, dialogKey.ToEnum<QuestState>());
            }
        }
    }

    private static void HandleQuestState(Player player, Npc npc, QuestProperties questProperties, QuestState state)
    {
        switch (state)
        {
            case QuestState.Suggest:
                npc.ShowQuestDialog(player, questProperties.BeginDialogs, DialogConstants.QuestAcceptDeclineButtons, questProperties.Id);
                break;
            case QuestState.BeginYes:
                player.QuestDiary.AcceptQuest(questProperties);
                npc.ShowQuestDialog(player, questProperties.AcceptedDialogs, DialogConstants.QuestOkButtons, questProperties.Id);
                break;
            case QuestState.BeginNo:
                npc.ShowQuestDialog(player, questProperties.DeclinedDialogs, DialogConstants.QuestOkButtons, questProperties.Id);
                break;
            case QuestState.End:
                SuggestFinalizeQuest(player, npc, questProperties);
                break;
            case QuestState.EndCompleted:
                FinalizeQuest(player, npc, questProperties);
                break;
        }
    }

    private static void SuggestFinalizeQuest(Player player, Npc npc, QuestProperties questProperties)
    {
        Quest quest = player.QuestDiary.GetActiveQuest(questProperties.Id);

        if (quest is not null)
        {
            if (!npc.Name.Equals(questProperties.EndCharacter, StringComparison.OrdinalIgnoreCase) || !quest.CanFinish())
            {
                npc.ShowQuestDialog(player, quest.Properties.NotFinishedDialogs, DialogConstants.QuestOkButtons, quest.Id);
            }
            else
            {
                npc.ShowQuestDialog(player, quest.Properties.CompletedDialogs, DialogConstants.QuestFinishButtons, quest.Id);
            }
        }
    }

    private static void FinalizeQuest(Player player, Npc npc, QuestProperties questProperties)
    {
        Quest quest = player.QuestDiary.GetActiveQuest(questProperties.Id) ??
            throw new InvalidOperationException($"Cannot find quest with id '{questProperties.Id}' for player '{player}'.");

        player.QuestDiary.CompleteQuest(quest);
        npc.SuggestAvailableQuest(player);
    }
}
