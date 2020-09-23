using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Game.Protocol.Snapshots;
using Rhisis.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public sealed class DialogSystem : IDialogSystem
    {
        private readonly ILogger<DialogSystem> _logger;

        public DialogSystem(ILogger<DialogSystem> logger)
        {
            _logger = logger;
        }

        public void OpenNpcDialog(INpc npc, IPlayer player, string dialogKey, int questId = 0)
        {
            if (string.IsNullOrEmpty(dialogKey))
            {
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
                    // TODO: quest
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
                        npc.Speak(npc.Dialog.ByeText);
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
