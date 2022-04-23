using Microsoft.Extensions.Options;
using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Factories;
using Rhisis.Abstractions.Features;
using Rhisis.Abstractions.Protocol;
using Rhisis.Abstractions.Resources;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Protocol.Snapshots.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class QuestDiary : GameFeature, IQuestDiary
    {
        private readonly IPlayer _player;
        private readonly IGameResources _gameResources;
        private readonly IEntityFactory _entityFactory;
        private readonly WorldOptions _worldServerConfiguration;
        private readonly IList<IQuest> _quests;

        public IEnumerable<IQuest> ActiveQuests => _quests.Where(x => !x.IsFinished);

        public IEnumerable<IQuest> CheckedQuests => ActiveQuests.Where(x => x.IsChecked);

        public IEnumerable<IQuest> CompletedQuests => _quests.Where(x => x.IsFinished);

        public QuestDiary(IPlayer player, IGameResources gameResources, IEntityFactory entityFactory, IOptions<WorldOptions> worldServerConfiguration)
        {
            _player = player;
            _gameResources = gameResources;
            _entityFactory = entityFactory;
            _worldServerConfiguration = worldServerConfiguration.Value;
            _quests = new List<IQuest>();
        }

        public IQuest GetActiveQuest(int questId) => ActiveQuests.FirstOrDefault(x => x.Id == questId);

        public bool HasQuest(int questId) => _quests.Any(x => x.Id == questId);
        
        public void Add(IQuest quest)
        {
            if (_quests.Contains(quest))
            {
                throw new InvalidOperationException($"Quest '{quest.Id}' for player with id '{quest.CharacterId}' already exists.");
            }

            _quests.Add(quest);
        }

        public void Remove(IQuest quest)
        {
            quest.IsDeleted = true;
        }

        public void Update(QuestActionType actionType, params object[] values)
        {
            foreach (IQuest quest in ActiveQuests)
            {
                bool updateQuest = false;

                if (actionType == QuestActionType.KillMonster)
                {
                    int killedMonsterId = Convert.ToInt32(values.ElementAtOrDefault(0));
                    short killedAmount = Convert.ToInt16(values.ElementAtOrDefault(1));

                    if (quest.Script.EndConditions.Monsters != null && quest.Script.EndConditions.Monsters.Any())
                    {
                        QuestMonster monsterToKill = quest.Script.EndConditions.Monsters.FirstOrDefault(x => _gameResources.GetDefinedValue(x.Id) == killedMonsterId);

                        if (quest.Monsters.ContainsKey(killedMonsterId) && quest.Monsters[killedMonsterId] < monsterToKill.Amount)
                        {
                            quest.Monsters[killedMonsterId] += killedAmount;
                            updateQuest = true;
                        }
                    }

                    IEnumerable<QuestItemDrop> questItemDrops = quest.Script.Drops.Where(x => _gameResources.GetDefinedValue(x.MonsterId) == killedMonsterId);

                    if (questItemDrops.Any())
                    {
                        foreach (QuestItemDrop questItem in questItemDrops)
                        {
                            // TODO: move this constant to configuration file
                            const long MaxDropChance = 3000000000;
                            long dropChance = RandomHelper.LongRandom(0, MaxDropChance);

                            if (dropChance < questItem.Probability * _worldServerConfiguration.Rates.Drop)
                            {
                                int itemId = _gameResources.GetDefinedValue(questItem.ItemId);
                                IItem item = _entityFactory.CreateItem(itemId, 0, ElementType.None, 0, quantity: questItem.Quantity);
                                int createdQuantity = _player.Inventory.CreateItem(item, item.Quantity, _player.CharacterId);

                                if (createdQuantity > 0)
                                {
                                    SendDefinedText(_player, DefineText.TID_EVE_REAPITEM, $"\"{item.Name ?? "[undefined]"}\"");
                                }
                                else
                                {
                                    IMapItem mapItem = _entityFactory.CreateMapItem(item, _player.MapLayer, _player, _player.Position);

                                    _player.MapLayer.AddItem(mapItem);
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
                    using var snapshot = new SetQuestSnapshot(_player, quest);
                    _player.Send(snapshot);
                }
            }
        }

        public void Serialize(IFFPacket packet)
        {
            packet.WriteByte((byte)ActiveQuests.Count());
            foreach (IQuest quest in ActiveQuests)
            {
                quest.Serialize(packet);
            }

            packet.WriteByte((byte)CompletedQuests.Count());
            foreach (IQuest quest in CompletedQuests)
            {
                packet.WriteInt16((short)quest.Id);
            }

            packet.WriteByte((byte)CheckedQuests.Count());
            foreach (IQuest quest in CheckedQuests)
            {
                packet.WriteInt16((short)quest.Id);
            }
        }

        public IEnumerator<IQuest> GetEnumerator() => _quests.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _quests.GetEnumerator();
    }
}
