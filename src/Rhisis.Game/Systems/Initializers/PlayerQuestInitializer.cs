using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common.Resources.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable]
    public sealed class PlayerQuestInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;

        public int Order => 3;

        public PlayerQuestInitializer(IRhisisDatabase database, IGameResources gameResources)
        {
            _database = database;
            _gameResources = gameResources;
        }

        public void Load(IPlayer player)
        {
            IEnumerable<IQuest> playerQuests = _database.Quests.Where(x => x.CharacterId == player.CharacterId)
                .AsNoTracking()
                .AsEnumerable()
                .Select(x =>
                {
                    IQuestScript questScript = _gameResources.Quests.GetValueOrDefault(x.QuestId);

                    if (questScript == null)
                    {
                        return null;
                    }

                    var quest = new Quest(questScript, x.CharacterId, x.Id)
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

            foreach (IQuest quest in playerQuests)
            {
                player.Quests.Add(quest);
            }
        }

        /// <inheritdoc />
        public void Save(IPlayer player)
        {
            var questsSet = from x in _database.Quests.Where(x => x.CharacterId == player.CharacterId).ToList()
                            join q in player.Quests on
                             new { x.QuestId, x.CharacterId }
                             equals
                             new { QuestId = q.Id, q.CharacterId }
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

            foreach (IQuest quest in player.Quests)
            {
                if (!quest.DatabaseQuestId.HasValue)
                {
                    _database.Quests.Add(new DbQuest
                    {
                        CharacterId = player.CharacterId,
                        QuestId = quest.Id,
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
