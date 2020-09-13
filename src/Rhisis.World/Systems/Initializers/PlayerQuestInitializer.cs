using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.World.Game;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerQuestInitializer : IGameSystemLifeCycle
    {
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;

        public int Order => 3;

        public PlayerQuestInitializer(IRhisisDatabase database, IGameResources gameResources)
        {
            _database = database;
            _gameResources = gameResources;
        }

        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<QuestInfo> playerQuests = _database.Quests.Where(x => x.CharacterId == player.PlayerData.Id)
                .AsNoTracking()
                .AsEnumerable()
                .Select(x =>
                {
                    IQuestScript questScript = _gameResources.Quests.GetValueOrDefault(x.QuestId);

                    if (questScript == null)
                    {
                        return null;
                    }

                    var quest = new QuestInfo(x.QuestId, x.CharacterId, questScript, x.Id)
                    {
                        IsChecked = x.IsChecked,
                        IsFinished = x.Finished,
                        StartTime = x.StartTime,
                        IsPatrolDone = x.IsPatrolDone
                    };

                    if (questScript.EndConditions.Monsters != null && questScript.EndConditions.Monsters.Any())
                    {
                        quest.Monsters = new Dictionary<int, short>
                        {
                            { _gameResources.GetDefinedValue(questScript.EndConditions.Monsters.ElementAtOrDefault(0)?.Id), (short)x.MonsterKilled1 },
                            { _gameResources.GetDefinedValue(questScript.EndConditions.Monsters.ElementAtOrDefault(1)?.Id), (short)x.MonsterKilled2 }
                        };
                    }

                    return quest;
                })
                .Where(x => x != null);

            player.QuestDiary = new QuestDiaryComponent(playerQuests);
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            var questsSet = from x in _database.Quests.Where(x => x.CharacterId == player.PlayerData.Id).ToList()
                            join q in player.QuestDiary on
                             new { x.QuestId, x.CharacterId }
                             equals
                             new { q.QuestId, q.CharacterId }
                            select new { DbQuest = x, PlayerQuest = q };

            foreach (var questSet in questsSet)
            {
                questSet.DbQuest.IsChecked = questSet.PlayerQuest.IsFinished ? false : questSet.PlayerQuest.IsChecked;
                questSet.DbQuest.IsDeleted = questSet.PlayerQuest.IsDeleted;
                questSet.DbQuest.IsPatrolDone = questSet.PlayerQuest.IsPatrolDone;
                questSet.DbQuest.Finished = questSet.PlayerQuest.IsFinished;

                if (questSet.PlayerQuest.Monsters != null)
                {
                    questSet.DbQuest.MonsterKilled1 = questSet.PlayerQuest.Monsters.ElementAtOrDefault(0).Value;
                    questSet.DbQuest.MonsterKilled2 = questSet.PlayerQuest.Monsters.ElementAtOrDefault(1).Value;
                }

                _database.Quests.Update(questSet.DbQuest);
            }

            foreach (QuestInfo quest in player.QuestDiary)
            {
                if (!quest.DatabaseQuestId.HasValue)
                {
                    _database.Quests.Add(new DbQuest
                    {
                        CharacterId = player.PlayerData.Id,
                        QuestId = quest.QuestId,
                        StartTime = quest.StartTime,
                        MonsterKilled1 = quest.Monsters?.ElementAtOrDefault(0).Value ?? default,
                        MonsterKilled2 = quest.Monsters?.ElementAtOrDefault(1).Value ?? default,
                        IsPatrolDone = quest.IsPatrolDone,
                        IsChecked = quest.IsFinished ? false : quest.IsChecked,
                        Finished = quest.IsFinished
                    });
                }
            }

            _database.SaveChanges();
        }
    }
}
