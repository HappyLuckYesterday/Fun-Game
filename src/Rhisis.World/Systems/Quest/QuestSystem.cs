﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Drop;
using Rhisis.World.Systems.Experience;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Job;
using Rhisis.World.Systems.PlayerData;
using Rhisis.World.Systems.Skills;
using Rhisis.World.Systems.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Quest
{
    [Injectable]
    public sealed class QuestSystem : IQuestSystem
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
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IPlayerDataSystem _playerDataSystem;
        private readonly IInventorySystem _inventorySystem;
        private readonly IExperienceSystem _experienceSystem;
        private readonly IJobSystem _jobSystem;
        private readonly IStatisticsSystem _statisticsSystem;
        private readonly ISkillSystem _skillSystem;
        private readonly IDropSystem _dropSystem;
        private readonly IQuestPacketFactory _questPacketFactory;
        private readonly INpcDialogPacketFactory _npcDialogPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;
        private readonly IItemFactory _itemFactory;

        /// <inheritdoc />
        public int Order => 3;

        public QuestSystem(ILogger<QuestSystem> logger, IGameResources gameResources, IOptions<WorldConfiguration> worldServerConfiguration,
            IPlayerDataSystem playerDataSystem, IInventorySystem inventorySystem, IExperienceSystem experienceSystem, IJobSystem jobSystem,
            IStatisticsSystem statisticsSystem, ISkillSystem skillSystem, IDropSystem dropSystem,
            IQuestPacketFactory questPacketFactory, INpcDialogPacketFactory npcDialogPacketFactory, ITextPacketFactory textPacketFactory, IItemFactory itemFactory)
        {
            _logger = logger;
            _gameResources = gameResources;
            _worldConfiguration = worldServerConfiguration.Value;
            _playerDataSystem = playerDataSystem;
            _inventorySystem = inventorySystem;
            _experienceSystem = experienceSystem;
            _jobSystem = jobSystem;
            _statisticsSystem = statisticsSystem;
            _skillSystem = skillSystem;
            _dropSystem = dropSystem;
            _questPacketFactory = questPacketFactory;
            _npcDialogPacketFactory = npcDialogPacketFactory;
            _textPacketFactory = textPacketFactory;
            _itemFactory = itemFactory;
        }

        /// <inheritdoc />
        public bool CanStartQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            if (quest == null)
            {
                return false;
            }

            if (player.QuestDiary.HasQuest(quest.Id))
            {
                return false;
            }

            if (!npc.Object.Name.Equals(quest.StartCharacter, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            int previousQuestId = _gameResources.GetDefinedValue(quest.StartRequirements.PreviousQuestId);

            if (previousQuestId != 0 && !player.QuestDiary.CompletedQuests.Any(x => x.QuestId == previousQuestId))
            {
                _logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player '{player}'. Did not finished quest '{quest.StartRequirements.PreviousQuestId}'.");
                return false;
            }

            if (player.Object.Level < quest.StartRequirements.MinLevel || player.Object.Level > quest.StartRequirements.MaxLevel)
            {
                _logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player: '{player}'. Level too low or too high.");
                return false;
            }

            if (quest.StartRequirements.Jobs != null && !quest.StartRequirements.Jobs.Contains(player.PlayerData.Job))
            {
                _logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player: '{player}'. Invalid job.");
                return false;
            }

            // TODO: add more checks

            return true;
        }

        /// <inheritdoc />
        public bool CanFinishQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            QuestInfo questToFinish = player.QuestDiary.ActiveQuests.FirstOrDefault(x => x.QuestId == quest.Id);

            if (questToFinish != null)
            {
                if (!npc.Object.Name.Equals(quest.EndCharacter, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                // Check items
                if (quest.EndConditions.Items != null && quest.EndConditions.Items.Any())
                {
                    foreach (QuestItem questItem in quest.EndConditions.Items)
                    {
                        if (questItem.Sex == GenderType.Any || questItem.Sex == player.PlayerData.Gender)
                        {
                            Item inventoryItem = player.Inventory.GetItem(_gameResources.GetDefinedValue(questItem.Id));

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
                        int questMonsterId = _gameResources.GetDefinedValue(questMonster.Id);

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
        public void ProcessQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest, QuestStateType state)
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
        public void CheckQuest(IPlayerEntity player, int questId, bool checkedState)
        {
            QuestInfo quest = player.QuestDiary.GetActiveQuest(questId);

            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest), $"Cannot find quest with id '{questId}' for player '{player}'.");
            }

            if (quest.IsChecked == checkedState)
            {
                throw new InvalidOperationException($"{player} tried to hack quest check state.");
            }

            quest.IsChecked = !quest.IsChecked;

            _questPacketFactory.SendCheckedQuests(player, player.QuestDiary.CheckedQuests);
        }

        /// <inheritdoc />
        public void SendQuestsInfo(IPlayerEntity player, INpcEntity npc)
        {
            if (npc.Quests.Any())
            {
                IEnumerable<DialogLink> newQuestsLinks = from x in npc.Quests
                                                         where CanStartQuest(player, npc, x)
                                                         select CreateQuestLink(x, QuestStateType.Suggest);
                IEnumerable<DialogLink> questsInProgress = from x in npc.Quests
                                                           where player.QuestDiary.ActiveQuests.Any(y => y.QuestId == x.Id)
                                                           select CreateQuestLink(x, QuestStateType.End);

                _npcDialogPacketFactory.SendQuestDialogs(player, newQuestsLinks, questsInProgress);
            }
        }

        /// <inheritdoc />
        public void UpdateQuestDiary(IPlayerEntity player, QuestActionType actionType, params object[] values)
        {
            foreach (QuestInfo quest in player.QuestDiary.ActiveQuests)
            {
                if (!_gameResources.Quests.TryGetValue(quest.QuestId, out IQuestScript questScript))
                {
                    return;
                }

                bool updateQuest = false;

                if (actionType == QuestActionType.KillMonster)
                {
                    int killedMonsterId = Convert.ToInt32(values.ElementAtOrDefault(0));
                    short killedAmount = Convert.ToInt16(values.ElementAtOrDefault(1));

                    if (questScript.EndConditions.Monsters != null && questScript.EndConditions.Monsters.Any())
                    {
                        QuestMonster monsterToKill = questScript.EndConditions.Monsters.FirstOrDefault(x => _gameResources.GetDefinedValue(x.Id) == killedMonsterId);

                        if (quest.Monsters.ContainsKey(killedMonsterId) && quest.Monsters[killedMonsterId] < monsterToKill.Amount)
                        {
                            quest.Monsters[killedMonsterId] += killedAmount;
                            updateQuest = true;
                        }
                    }

                    IEnumerable<QuestItemDrop> questItemDrops = questScript.Drops.Where(x => _gameResources.GetDefinedValue(x.MonsterId) == killedMonsterId);

                    if (questItemDrops.Any())
                    {
                        foreach (QuestItemDrop questItem in questItemDrops)
                        {
                            long dropChance = RandomHelper.LongRandom(0, DropSystem.MaxDropChance);

                            if (dropChance < questItem.Probability * _worldConfiguration.Rates.Drop)
                            {
                                int itemId = _gameResources.GetDefinedValue(questItem.ItemId);
                                int createdQuantity = _inventorySystem.CreateItem(player, itemId, questItem.Quantity, player.PlayerData.Id);

                                if (createdQuantity > 0)
                                {
                                    ItemData itemData = _gameResources.Items.GetValueOrDefault(itemId);

                                    _textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_REAPITEM, $"\"{itemData?.Name ?? "[undefined]"}\"");
                                }
                                else
                                {
                                    _dropSystem.DropItem(player, itemId, player, questItem.Quantity);
                                }
                            }
                        }
                    }
                }

                if (actionType == QuestActionType.Patrol)
                {
                    // TODO
                }

                if (updateQuest)
                {
                    _questPacketFactory.SendQuest(player, quest);
                }
            }
        }

        /// <summary>
        /// Suggest a quest to the current player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to suggest.</param>
        private void SuggestQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            SendQuestDialog(player, npc, quest.Id, quest.BeginDialogs, AcceptDeclineButtons);
        }

        /// <summary>
        /// Accepts a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to accept.</param>
        private void AcceptQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            var acceptedQuest = new QuestInfo(quest.Id, player.PlayerData.Id, quest)
            {
                StartTime = DateTime.UtcNow,
                Monsters = quest.EndConditions.Monsters?.ToDictionary(x => _gameResources.GetDefinedValue(x.Id), x => (short)0)
            };

            player.QuestDiary.Add(acceptedQuest);

            _questPacketFactory.SendQuest(player, acceptedQuest);
            _textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_STARTQUEST, $"\"{_gameResources.GetText(quest.Title)}\"");
            SendQuestDialog(player, npc, quest.Id, quest.AcceptedDialogs, OkButtons);
        }

        /// <summary>
        /// Declines a quest suggestion.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Declined quest.</param>
        private void DeclineQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
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
        private void FinishQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
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
        private void FinalizeQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            QuestInfo questToFinish = player.QuestDiary.GetActiveQuest(quest.Id);

            if (questToFinish == null)
            {
                _logger.LogError($"Cannot find quest with id '{quest.Id}' for player '{player}'.");
                return;
            }

            // Check if player has enough space for reward items.
            if (quest.Rewards.Items != null && quest.Rewards.Items.Any())
            {
                IEnumerable<QuestItem> itemsForPlayer = quest.Rewards.Items.Where(x => x.Sex == player.PlayerData.Gender || x.Sex == GenderType.Any);

                if (player.Inventory.GetItemCount() + itemsForPlayer.Count() > player.Inventory.MaxStorageCapacity)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_QUEST_NOINVENTORYSPACE);
                    return;
                }

                foreach (QuestItem rewardItem in itemsForPlayer)
                {
                    int rewardItemId = _gameResources.GetDefinedValue(rewardItem.Id);

                    if (_gameResources.Items.TryGetValue(rewardItemId, out ItemData itemData))
                    {
                        Item itemToCreate = _itemFactory.CreateItem(rewardItemId, rewardItem.Refine, rewardItem.Element, rewardItem.ElementRefine);

                        _inventorySystem.CreateItem(player, itemToCreate, rewardItem.Quantity);
                        _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REAPITEM, $"\"{_gameResources.GetText(itemToCreate.Data.Name)}\"");
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
                        if (questItem.Sex == GenderType.Any || questItem.Sex == player.PlayerData.Gender)
                        {
                            InventoryItem inventoryItem = player.Inventory.GetItem(_gameResources.GetDefinedValue(questItem.Id));

                            if (inventoryItem != null)
                            {
                                _inventorySystem.DeleteItem(player, inventoryItem, questItem.Quantity);
                            }
                        }
                    }
                }
            }

            if (quest.Rewards.Gold > 0 && _playerDataSystem.IncreaseGold(player, quest.Rewards.Gold))
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REAPMONEY, 
                    quest.Rewards.Gold.ToString("###,###,###,###"), 
                    player.PlayerData.Gold.ToString("###,###,###,###"));
            }

            if (quest.Rewards.Experience > 0)
            {
                _experienceSystem.GiveExeperience(player, quest.Rewards.Experience);
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REAPEXP);
            }

            if (quest.Rewards.HasJobReward())
            {
                DefineJob.Job newJob = quest.Rewards.GetJob(player);

                if (newJob != DefineJob.Job.JOB_VAGRANT)
                {
                    _jobSystem.ChangeJob(player, newJob);
                }
            }

            if (quest.Rewards.Restat)
            {
                _statisticsSystem.Restat(player);
            }

            if (quest.Rewards.Reskill)
            {
                _skillSystem.Reskill(player);
            }

            if (quest.Rewards.SkillPoints > 0)
            {
                _skillSystem.AddSkillPoints(player, quest.Rewards.SkillPoints);
            }

            questToFinish.IsFinished = true;
            questToFinish.State = QuestStateType.Completed;

            _textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_ENDQUEST, $"\"{_gameResources.GetText(quest.Title)}\"");
            _questPacketFactory.SendQuest(player, questToFinish);

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

            _npcDialogPacketFactory.SendCloseDialog(player);
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
        private void SendQuestDialog(IPlayerEntity player, INpcEntity npc, int questId, IEnumerable<string> questTexts, IEnumerable<DialogLink> buttons)
        {
            _npcDialogPacketFactory.SendDialog(player, GetQuestDialogsTexts(questTexts), npc.NpcData.Dialog?.Links, buttons, questId);
            SendQuestsInfo(player, npc);
        }
    }
}
