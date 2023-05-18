using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using Rhisis.Game.Resources.Properties.Dialogs;
using Rhisis.Game.Resources.Properties.Quests;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rhisis.Game.Entities;

[DebuggerDisplay("{Name}")]
public sealed class Npc : WorldObject
{
    private static float OralTextRadius = 50f;

    private long _lastSpeakTime;

    public override WorldObjectType Type => WorldObjectType.Mover;

    public NpcProperties Properties { get; }

    public bool HasShop => Shop is not null;

    public ItemContainer[] Shop { get; }

    public bool HasDialog => Properties.HasDialog;

    public IEnumerable<QuestProperties> Quests { get; init; }

    public bool HasQuests => Quests.Any();
 
    public Npc(NpcProperties properties)
    {
        Properties = properties;
        Name = Properties.Id;

        if (Properties.HasShop)
        {
            Shop = new ItemContainer[Properties.Shop.Items.Length];
            for (int i = 0; i < Properties.Shop.Items.Length; i++)
            {
                Shop[i] = new ItemContainer(100);
                Dictionary<int, Item> shopItems = Properties.Shop.Items[i]
                    .Select((x, index) => new
                    {
                        ShopItem = x,
                        Slot = index
                    })
                    .ToDictionary(x => x.Slot, x =>
                    {
                        ItemProperties itemProperties = GameResources.Current.Items.Get(x.ShopItem.Id);

                        return new Item(itemProperties)
                        {
                            Refine = x.ShopItem.Refine,
                            Element = x.ShopItem.Element,
                            ElementRefine = x.ShopItem.ElementRefine,
                            Quantity = itemProperties.PackMax
                        };
                    });

                Shop[i].Initialize(shopItems);
            }
        }

        Quests = GameResources.Current.Quests
            .Where(q => !string.IsNullOrEmpty(q.StartCharacter) && q.StartCharacter.Equals(Name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Opens the NPC shop to the given player.
    /// </summary>
    /// <param name="target">Target player.</param>
    public void OpenShop(Player target)
    {
        if (!HasShop)
        {
            return;
        }

        using OpenNpcShopWindowSnapshot packet = new(this, Shop);

        target.Send(packet);
    }

    /// <summary>
    /// Updates the NPC logic.
    /// </summary>
    public void Update()
    {
        if (!IsSpawned)
        {
            return;
        }

        Speak();
    }

    /// <summary>
    /// Speaks to given text to the player.
    /// </summary>
    /// <param name="text">Text to speak.</param>
    /// <param name="player">Player.</param>
    public void Speak(string text, Player player)
    {
        using ChatSnapshot packet = new(this, text);

        player.Send(packet);
    }

    /// <summary>
    /// Show a dialog talk to the target player.
    /// </summary>
    /// <param name="targetPlayer">Target player.</param>
    /// <param name="texts">Dialog texts.</param>
    /// <param name="links">Additionnal dialog links.</param>
    /// <param name="buttons">Dialog buttons.</param>
    /// <param name="questId">Dialog quest id.</param>
    public void ShowDialog(Player targetPlayer, IEnumerable<string> texts = null, IEnumerable<DialogLink> links = null, IEnumerable<DialogLink> buttons = null, int questId = 0)
    {
        using FFSnapshot snapshots = new();

        snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_REMOVEALLKEY));

        if (texts != null && texts.Any())
        {
            foreach (string text in texts)
            {
                snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_SAY, text, questId));
            }
        }

        if (Properties.Dialog?.Links is not null && Properties.Dialog.Links.Any())
        {
            foreach (DialogLink link in Properties.Dialog.Links)
            {
                snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_ADDKEY, link.Title, link.Id, link.QuestId.GetValueOrDefault()));
            }
        }

        if (links != null && links.Any())
        {
            foreach (DialogLink link in links)
            {
                snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_ADDKEY, link.Title, link.Id, link.QuestId.GetValueOrDefault()));
            }
        }

        if (buttons != null && buttons.Any())
        {
            foreach (DialogLink buttonLink in buttons)
            {
                snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_ADDANSWER, buttonLink.Title, buttonLink.Id, buttonLink.QuestId.GetValueOrDefault(questId)));
            }
        }

        if (HasQuests)
        {
            IEnumerable<DialogLink> newQuestLinks = Quests
                .Where(targetPlayer.QuestDiary.CanStartQuest)
                .Select(q => new DialogLink(QuestState.Suggest.ToString(), GameResources.Current.GetText(q.Title), q.Id));
            IEnumerable<DialogLink> questsInProgressLinks = Quests
                .Where(q => targetPlayer.QuestDiary.HasActiveQuest(q.Id))
                .Select(q => new DialogLink(QuestState.End.ToString(), GameResources.Current.GetText(q.Title), q.Id));

            if (newQuestLinks is not null && newQuestLinks.Any())
            {
                foreach (DialogLink newQuestLink in newQuestLinks)
                {
                    snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_NEWQUEST, newQuestLink.Title, newQuestLink.Id, newQuestLink.QuestId.GetValueOrDefault()));
                }
            }

            if (questsInProgressLinks is not null && questsInProgressLinks.Any())
            {
                foreach (DialogLink currentQuestLink in questsInProgressLinks)
                {
                    snapshots.Merge(new DialogOptionSnapshot(targetPlayer, DialogOptions.FUNCTYPE_CURRQUEST, currentQuestLink.Title, currentQuestLink.Id, currentQuestLink.QuestId.GetValueOrDefault()));
                }
            }
        }

        targetPlayer.Send(snapshots);
    }

    /// <summary>
    /// Shows a quest dialog to the given player.
    /// </summary>
    /// <param name="player">Player.</param>
    /// <param name="texts">Quest dialog texts.</param>
    /// <param name="buttons">Quest dialog buttons.</param>
    /// <param name="questId">Quest id.</param>
    public void ShowQuestDialog(Player player, IEnumerable<string> texts, IEnumerable<DialogLink> buttons, int questId)
    {
        IEnumerable<string> questDialogs = texts.Select(GameResources.Current.GetText);

        ShowDialog(player, questDialogs, Properties.Dialog?.Links, buttons, questId);
    }

    /// <summary>
    /// Closes the current dialog box.
    /// </summary>
    /// <param name="player">Player.</param>
    public void CloseDialog(Player player)
    {
        using DialogOptionSnapshot snapshot = new(player, DialogOptions.FUNCTYPE_EXIT);

        player.Send(snapshot);
    }

    /// <summary>
    /// Suggests an available quest to the given player.
    /// </summary>
    /// <param name="player">Player.</param>
    /// <returns>True if a quest is suggested; false otherwise.</returns>
    public bool SuggestAvailableQuest(Player player)
    {
        IEnumerable<QuestProperties> availableQuests = Quests.Where(player.QuestDiary.CanStartQuest);

        if (availableQuests.Any())
        {
            QuestProperties quest = availableQuests.First();

            ShowQuestDialog(player, quest.BeginDialogs, DialogConstants.QuestAcceptDeclineButtons, quest.Id);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Suggests to finish a quest to the given player.
    /// </summary>
    /// <param name="player">Player.</param>
    /// <returns>True if the player can finish a quest; false otherwise.</returns>
    public bool SuggestFinalizeQuest(Player player)
    {
        IEnumerable<Quest> playerQuestToFinalize = player.QuestDiary.ActiveQuests
            .Where(q => q.CanFinish() && q.Properties.EndCharacter.Equals(Name, StringComparison.OrdinalIgnoreCase));

        if (playerQuestToFinalize.Any())
        {
            Quest quest = playerQuestToFinalize.First();
            ShowQuestDialog(player, quest.Properties.CompletedDialogs, DialogConstants.QuestFinishButtons, quest.Id);

            return true;
        }

        return false;
    }

    private void Speak()
    {
        if (Properties.Dialog is not null && !string.IsNullOrEmpty(Properties.Dialog.OralText))
        {
            if (_lastSpeakTime <= Time.TimeInSeconds())
            {
                IEnumerable<Player> playersAround = VisibleObjects
                    .Where(x => x is Player && Position.IsInCircle(x.Position, OralTextRadius))
                    .Cast<Player>();

                if (playersAround.Any())
                {
                    foreach (Player player in playersAround)
                    {
                        Speak(Properties.Dialog.OralText, player);
                    }
                }

                _lastSpeakTime = Time.TimeInSeconds() + FFRandom.Random(10, 15);
            }
        }
    }
}