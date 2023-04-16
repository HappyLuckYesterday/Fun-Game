using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources.Properties.Dialogs;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Collections.Generic;
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

        string dialogKey = packet.DialogKey;
        int questId = packet.QuestId;

        if (string.IsNullOrEmpty(dialogKey))
        {
            // TODO: check if has quests

            if (npc.HasDialog)
            {
                SendNpcDialog(Player, npc.Properties.Dialog.IntroText, npc.Properties.Dialog.Links);
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
                        CloseDialog(Player);
                    }
                }
                else
                {
                    if (dialogKey == DialogConstants.Bye)
                    {
                        CloseDialog(Player);
                        npc.Speak(npc.Properties.Dialog.ByeText, Player);
                    }
                    else
                    {
                        DialogLink dialogLink = (npc.Properties.Dialog.Links?.FirstOrDefault(x => x.Id == dialogKey)) 
                            ?? throw new InvalidOperationException($"Cannot find dialog key: '{dialogKey}' for NPC '{npc.Name}'.");
                        
                        SendNpcDialog(Player, dialogLink.Texts, npc.Properties.Dialog.Links);
                    }
                }
            }
            else
            {
                // TODO: Handle quest dialog
            }
        }
    }

    private static void SendNpcDialog(Player player, IEnumerable<string> texts, IEnumerable<DialogLink> links, int questId = 0)
    {
        using FFSnapshot snapshots = new();

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

        player.Send(snapshots);
    }

    private static void CloseDialog(Player player)
    {
        using DialogOptionSnapshot snapshot = new(player, DialogOptions.FUNCTYPE_EXIT);

        player.Send(snapshot);
    }
}
