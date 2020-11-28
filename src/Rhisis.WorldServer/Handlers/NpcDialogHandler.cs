using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Game.Protocol.Snapshots;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class NpcDialogHandler
    {
        private readonly ILogger<NpcDialogHandler> _logger;
        private readonly IQuestSystem _questSystem;

        public NpcDialogHandler(ILogger<NpcDialogHandler> logger, IQuestSystem questSystem)
        {
            _logger = logger;
            _questSystem = questSystem;
        }

        /// <summary>
        /// Opens a dialog script.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming <see cref="DialogPacket"/>.</param>
        [HandlerAction(PacketType.SCRIPTDLG)]
        public void OnDialogScript(IPlayer player, DialogPacket packet)
        {
            if (packet.ObjectId <= 0)
            {
                throw new ArgumentException("Invalid object id.");
            }

            INpc npc = player.VisibleObjects.OfType<INpc>().FirstOrDefault(x => x.Id == packet.ObjectId);

            if (npc == null)
            {
                throw new ArgumentException($"Cannot find NPC with object id: {packet.ObjectId}");
            }

            string dialogKey = packet.DialogKey;
            int questId = packet.QuestId;

            if (string.IsNullOrEmpty(dialogKey))
            {
                if (npc.HasQuests)
                {
                    IEnumerable<IQuestScript> availableQuests = npc.Quests.Where(x => _questSystem.CanStartQuest(player, npc, x));

                    if (availableQuests.Count() == 1)
                    {
                        IQuestScript firstQuest = availableQuests.First();
                        var questState = _questSystem.CanStartQuest(player, npc, firstQuest) ? QuestStateType.Suggest : QuestStateType.End;

                        _questSystem.ProcessQuest(player, npc, firstQuest, questState);
                        return;
                    }
                }

                IEnumerable<IQuestScript> questsToBeFinished = player.Quests.ActiveQuests.Where(x => npc.Name.Equals(x.Script.EndCharacter, StringComparison.OrdinalIgnoreCase)).Select(x => x.Script);

                if (questsToBeFinished.Any())
                {
                    _questSystem.ProcessQuest(player, npc, questsToBeFinished.First(), QuestStateType.End);
                    return;
                }

                if (!npc.HasDialog)
                {
                    throw new InvalidOperationException($"NPC '{npc.Key}' doesn't have a dialog.");
                }

                // TODO: quest

                SendNpcDialog(player, npc.Dialog.IntroText, npc.Dialog.Links);
            }
            else
            {
                if (questId != 0)
                {
                    // Check if the quest exists for the current NPC
                    IQuestScript quest = npc.Quests.FirstOrDefault(x => x.Id == questId);

                    if (quest == null)
                    {
                        // If not, check if the npc is the end character of player's active quest
                        quest = player.Quests.ActiveQuests.Where(x => x.Id == questId && npc.Name.Equals(x.Script.EndCharacter, StringComparison.OrdinalIgnoreCase)).Select(x => x.Script).FirstOrDefault();

                        if (quest == null)
                        {
                            _logger.LogError($"Cannot find quest with id '{questId}' at npc '{npc}' for player '{player}'.");
                            return;
                        }
                    }

                    _questSystem.ProcessQuest(player, npc, quest, dialogKey.ToEnum<QuestStateType>());
                }
                else
                {
                    if (!npc.HasDialog)
                    {
                        _logger.LogError($"NPC '{npc.Name}' doesn't have a dialog.");

                        if (dialogKey == DialogConstants.Bye)
                        {
                            CloseDialog(player);
                        }

                        return;
                    }

                    if (dialogKey == DialogConstants.Bye)
                    {
                        CloseDialog(player);
                        npc.Chat.Speak(npc.Dialog.ByeText);
                    }
                    else
                    {
                        DialogLink dialogLink = npc.Dialog.Links?.FirstOrDefault(x => x.Id == dialogKey);

                        if (dialogLink == null)
                        {
                            _logger.LogError($"Cannot find dialog key: '{dialogKey}' for NPC '{npc.Name}'.");
                            return;
                        }

                        SendNpcDialog(player, dialogLink.Texts, npc.Dialog.Links);
                    }
                }
            }
        }

        private void SendNpcDialog(IPlayer player, IEnumerable<string> texts, IEnumerable<DialogLink> links, int questId = 0)
        {
            using var snapshots = new FFSnapshot();

            snapshots.Merge(new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_REMOVEALLKEY));

            if (texts != null && texts.Any())
            {
                foreach (string text in texts)
                {
                    snapshots.Merge(new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_SAY, text, questId));
                }
            }

            if (links != null && links.Any())
            {
                foreach (DialogLink link in links)
                {
                    snapshots.Merge(new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_ADDKEY, link.Title, link.Id, link.QuestId.GetValueOrDefault(questId)));
                }
            }

            player.Connection.Send(snapshots);
        }

        private void CloseDialog(IPlayer player)
        {
            using var snapshot = new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_EXIT);

            player.Connection.Send(snapshot);
        }
    }
}