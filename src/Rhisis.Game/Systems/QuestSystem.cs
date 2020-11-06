using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Game.Protocol.Snapshots;
using Rhisis.Game.Protocol.Snapshots.Quests;
using Rhisis.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public sealed class QuestSystem : GameFeature, IQuestSystem
    {
        private static readonly IEnumerable<DialogLink> AcceptDeclineButtons = new List<DialogLink>
        {
            new DialogLink(QuestStateType.BeginYes.ToString(), DialogConstants.Yes),
            new DialogLink(QuestStateType.BeginNo.ToString(), DialogConstants.No)
        };
        private static readonly IEnumerable<DialogLink> OkButtons = new List<DialogLink>
        {
            new DialogLink(DialogConstants.Bye, DialogConstants.Ok, 0)
        };
        private static readonly IEnumerable<DialogLink> FinishButtons = new List<DialogLink>
        {
            new DialogLink(QuestStateType.EndCompleted.ToString(), DialogConstants.Ok)
        };

        private readonly ILogger<QuestSystem> _logger;
        private readonly IGameResources _gameResources;
        private readonly IEntityFactory _entityFactory;

        /// <inheritdoc />
        public int Order => 3;

        public QuestSystem(ILogger<QuestSystem> logger, IGameResources gameResources, IEntityFactory entityFactory)
        {
            _logger = logger;
            _gameResources = gameResources;
            _entityFactory = entityFactory;
        }

        /// <inheritdoc />
        public bool CanStartQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            if (quest == null)
            {
                return false;
            }

            if (player.Quests.HasQuest(quest.Id))
            {
                return false;
            }

            if (!npc.Name.Equals(quest.StartCharacter, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var previousQuestId = _gameResources.GetDefinedValue(quest.StartRequirements.PreviousQuestId);

            if (previousQuestId != 0 && !player.Quests.CompletedQuests.Any(x => x.Id == previousQuestId))
            {
                _logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player '{player}'. Did not finished quest '{quest.StartRequirements.PreviousQuestId}'.");
                return false;
            }

            if (player.Level < quest.StartRequirements.MinLevel || player.Level > quest.StartRequirements.MaxLevel)
            {
                _logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player: '{player}'. Level too low or too high.");
                return false;
            }

            if (quest.StartRequirements.Jobs != null && !quest.StartRequirements.Jobs.Contains(player.Job.Id))
            {
                _logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player: '{player}'. Invalid job.");
                return false;
            }

            // TODO: add more checks

            return true;
        }

        /// <inheritdoc />
        public bool CanFinishQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            IQuest questToFinish = player.Quests.GetActiveQuest(quest.Id);

            if (questToFinish != null)
            {
                if (!npc.Name.Equals(quest.EndCharacter, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                // Check items
                if (quest.EndConditions.Items != null && quest.EndConditions.Items.Any())
                {
                    foreach (QuestItem questItem in quest.EndConditions.Items)
                    {
                        if (questItem.Sex == GenderType.Any || questItem.Sex == player.Appearence.Gender)
                        {
                            IItem inventoryItem = player.Inventory.GetItem(x => x.Id == _gameResources.GetDefinedValue(questItem.Id));

                            if (inventoryItem == null || inventoryItem.Quantity < questItem.Quantity)
                            {
                                return false;
                            }
                        }
                    }
                }

                // Check monsters
                if (quest.EndConditions.Monsters != null && quest.EndConditions.Monsters.Any())
                {
                    foreach (QuestMonster questMonster in quest.EndConditions.Monsters)
                    {
                        var questMonsterId = _gameResources.GetDefinedValue(questMonster.Id);

                        if (questToFinish.Monsters.TryGetValue(questMonsterId, out short killedQuantity))
                        {
                            if (killedQuantity < questMonster.Amount)
                            {
                                return false;
                            }
                        }
                    }
                }

                // TODO: Check region patrols
            }

            return true;
        }

        /// <inheritdoc />
        public void ProcessQuest(IPlayer player, INpc npc, IQuestScript quest, QuestStateType state)
        {
            switch (state)
            {
                case QuestStateType.Suggest:
                    SuggestQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginYes:
                    AcceptQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginNo:
                    DeclineQuest(player, npc, quest);
                    break;
                case QuestStateType.End:
                    FinishQuest(player, npc, quest);
                    break;
                case QuestStateType.EndCompleted:
                    FinalizeQuest(player, npc, quest);
                    break;
                default:
                    _logger.LogError($"Received unknown dialog quest state: {state}.");
                    break;
            }
        }

        /// <inheritdoc />
        public void SendQuestsInfo(IPlayer player, INpc npc)
        {
            if (npc.Quests.Any())
            {
                IEnumerable<DialogLink> newQuestsLinks = from x in npc.Quests
                                                         where CanStartQuest(player, npc, x)
                                                         select CreateQuestLink(x, QuestStateType.Suggest);
                IEnumerable<DialogLink> questsInProgress = from x in npc.Quests
                                                           where player.Quests.ActiveQuests.Any(y => y.Id == x.Id)
                                                           select CreateQuestLink(x, QuestStateType.End);

                SendQuestsDialogs(player, newQuestsLinks, questsInProgress);
            }
        }

        /// <summary>
        /// Suggest a quest to the current player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to suggest.</param>
        private void SuggestQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            SendQuestDialog(player, npc, quest.Id, quest.BeginDialogs, AcceptDeclineButtons);
        }

        /// <summary>
        /// Accepts a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to accept.</param>
        private void AcceptQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            var acceptedQuest = new Quest(quest, player.CharacterId)
            {
                Monsters = quest.EndConditions.Monsters?.ToDictionary(x => _gameResources.GetDefinedValue(x.Id), x => (short)0)
            };

            player.Quests.Add(acceptedQuest);

            SendQuest(player, acceptedQuest);
            SendDefinedText(player, DefineText.TID_EVE_STARTQUEST, $"\"{_gameResources.GetText(quest.Title)}\"");
            SendQuestDialog(player, npc, quest.Id, quest.AcceptedDialogs, OkButtons);
        }

        /// <summary>
        /// Declines a quest suggestion.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Declined quest.</param>
        private void DeclineQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            SendQuestDialog(player, npc, quest.Id, quest.DeclinedDialogs, OkButtons);
        }

        /// <summary>
        /// Verify that the player can finish the quest.
        /// If the player can finish the quest, sends the completed dialog.
        /// If not, the failure dialog is sent.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Current quest.</param>
        private void FinishQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            if (!CanFinishQuest(player, npc, quest))
            {
                SendQuestDialog(player, npc, quest.Id, quest.NotFinishedDialogs, OkButtons);
                return;
            }

            SendQuestDialog(player, npc, quest.Id, quest.CompletedDialogs, FinishButtons);
        }

        /// <summary>
        /// Finalize the quest and give the reward to the player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Current quest.</param>
        private void FinalizeQuest(IPlayer player, INpc npc, IQuestScript quest)
        {
            IQuest questToFinish = player.Quests.GetActiveQuest(quest.Id);

            if (questToFinish == null)
            {
                _logger.LogError($"Cannot find quest with id '{quest.Id}' for player '{player}'.");
                return;
            }

            // Check if player has enough space for reward items.
            if (quest.Rewards.Items != null && quest.Rewards.Items.Any())
            {
                IEnumerable<QuestItem> itemsForPlayer = quest.Rewards.Items.Where(x => x.Sex == player.Appearence.Gender || x.Sex == GenderType.Any);

                if (player.Inventory.GetItemCount() + itemsForPlayer.Count() > player.Inventory.StorageCapacity)
                {
                    SendDefinedText(player, DefineText.TID_QUEST_NOINVENTORYSPACE);
                    return;
                }

                foreach (QuestItem rewardItem in itemsForPlayer)
                {
                    var rewardItemId = _gameResources.GetDefinedValue(rewardItem.Id);

                    if (_gameResources.Items.TryGetValue(rewardItemId, out ItemData itemData))
                    {
                        IItem itemToCreate = _entityFactory.CreateItem(rewardItemId, rewardItem.Refine, rewardItem.Element, rewardItem.ElementRefine);

                        player.Inventory.CreateItem(itemToCreate, rewardItem.Quantity);
                        SendDefinedText(player, DefineText.TID_GAME_REAPITEM, $"\"{_gameResources.GetText(itemToCreate.Data.Name)}\"");
                    }
                }
            }

            // Remove quest items from inventory
            if (quest.EndConditions.Items != null && quest.EndConditions.Items.Any())
            {
                foreach (QuestItem questItem in quest.EndConditions.Items)
                {
                    if (questItem.Remove)
                    {
                        if (questItem.Sex == GenderType.Any || questItem.Sex == player.Appearence.Gender)
                        {
                            IItem inventoryItem = player.Inventory.GetItem(x => x.Id == _gameResources.GetDefinedValue(questItem.Id));

                            if (inventoryItem != null)
                            {
                                player.Inventory.DeleteItem(inventoryItem, questItem.Quantity);
                            }
                        }
                    }
                }
            }

            if (quest.Rewards.Gold > 0 && player.Gold.Increase(quest.Rewards.Gold))
            {
                SendDefinedText(player, DefineText.TID_GAME_REAPMONEY,
                    quest.Rewards.Gold.ToString("###,###,###,###"),
                    player.Gold.Amount.ToString("###,###,###,###"));
            }

            if (quest.Rewards.Experience > 0)
            {
                player.Experience.Increase(quest.Rewards.Experience);
                SendDefinedText(player, DefineText.TID_GAME_REAPEXP);
            }

            if (quest.Rewards.HasJobReward())
            {
                DefineJob.Job newJob = quest.Rewards.GetJob(player);

                if (newJob != DefineJob.Job.JOB_VAGRANT)
                {
                    //_jobSystem.ChangeJob(player, newJob);
                }
            }

            if (quest.Rewards.Restat)
            {
                player.Statistics.Restat();
            }

            if (quest.Rewards.Reskill)
            {
                //_skillSystem.Reskill(player);
            }

            if (quest.Rewards.SkillPoints > 0)
            {
                //_skillSystem.AddSkillPoints(player, quest.Rewards.SkillPoints);
            }

            questToFinish.IsFinished = true;
            questToFinish.State = QuestStateType.Completed;

            SendDefinedText(player, DefineText.TID_EVE_ENDQUEST, $"\"{_gameResources.GetText(quest.Title)}\"");
            SendQuest(player, questToFinish);

            if (npc.Quests.Any())
            {
                IEnumerable<IQuestScript> availableQuests = npc.Quests.Where(x => CanStartQuest(player, npc, x));

                if (availableQuests.Count() == 1)
                {
                    IQuestScript firstQuest = availableQuests.First();
                    var questState = CanStartQuest(player, npc, firstQuest) ? QuestStateType.Suggest : QuestStateType.End;

                    ProcessQuest(player, npc, firstQuest, questState);
                    return;
                }
            }

            using var closeDialogSnapshot = new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_EXIT);
            player.Send(closeDialogSnapshot);
        }

        /// <summary>
        /// Creates a new quest <see cref="DialogLink"/>.
        /// </summary>
        /// <param name="quest">Quest.</param>
        /// <param name="questState">Quest state.</param>
        /// <returns>Quest <see cref="DialogLink"/>.</returns>
        private DialogLink CreateQuestLink(IQuestScript quest, QuestStateType questState)
            => new DialogLink(questState.ToString(), _gameResources.GetText(quest.Title), quest.Id);

        /// <summary>
        /// Gets the quest dialog texts.
        /// </summary>
        /// <param name="questDialogsKeys">Quest dialog keys.</param>
        /// <returns>Quest dialog texts.</returns>
        private IEnumerable<string> GetQuestDialogsTexts(IEnumerable<string> questDialogsKeys)
            => questDialogsKeys.Select(x => _gameResources.GetText(x));

        /// <summary>
        /// Sends a quest dialog
        /// </summary>
        /// <param name="player"></param>
        /// <param name="npc"></param>
        /// <param name="questId"></param>
        /// <param name="questTexts"></param>
        /// <param name="buttons"></param>
        private void SendQuestDialog(IPlayer player, INpc npc, int questId, IEnumerable<string> questTexts, IEnumerable<DialogLink> buttons)
        {
            SendNpcDialog(player, GetQuestDialogsTexts(questTexts), npc.Dialog?.Links, buttons, questId);
            SendQuestsInfo(player, npc);
        }

        private void SendQuestsDialogs(IPlayer player, IEnumerable<DialogLink> newQuests, IEnumerable<DialogLink> currentQuests)
        {
            using var packet = new FFSnapshot();

            if (newQuests != null)
            {
                foreach (DialogLink newQuestLink in newQuests)
                {
                    packet.Merge(new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_NEWQUEST, newQuestLink.Title, newQuestLink.Id, newQuestLink.QuestId.GetValueOrDefault()));
                }
            }

            if (currentQuests != null)
            {
                foreach (DialogLink currentQuestLink in currentQuests)
                {
                    packet.Merge(new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_CURRQUEST, currentQuestLink.Title, currentQuestLink.Id, currentQuestLink.QuestId.GetValueOrDefault()));
                }
            }

            player.Send(packet);
        }

        private void SendNpcDialog(IPlayer player, IEnumerable<string> texts, IEnumerable<DialogLink> links, IEnumerable<DialogLink> buttons, int questId = 0)
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

            if (buttons != null)
            {
                foreach (DialogLink buttonLink in buttons)
                {
                    snapshots.Merge(new DialogOptionSnapshot(player, DialogOptions.FUNCTYPE_ADDANSWER, buttonLink.Title, buttonLink.Id, buttonLink.QuestId.GetValueOrDefault(questId)));
                }
            }

            player.Connection.Send(snapshots);
        }

        private void SendQuest(IPlayer player, IQuest quest)
        {
            using var questSnapshot = new SetQuestSnapshot(player, quest);

            player.Send(questSnapshot);
        }
    }
}
