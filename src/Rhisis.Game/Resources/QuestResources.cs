using Microsoft.Extensions.Logging;
using NLua;
using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Game.Extensions;
using Rhisis.Game.Resources.Properties.Quests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources;

public sealed class QuestResources
{
    private readonly ILogger<QuestResources> _logger;
    private readonly ConcurrentDictionary<string, int> _defines;
    private readonly ConcurrentDictionary<int, QuestProperties> _quests = new();
    private readonly ConcurrentDictionary<string, QuestProperties> _questByIdentifiers = new();

    public QuestResources(ILogger<QuestResources> logger, ConcurrentDictionary<string, int> defines)
    {
        _logger = logger;
        _defines = defines;
    }

    /// <summary>
    /// Gets the quest properties based on the given quest id.
    /// </summary>
    /// <param name="questId">Quest id.</param>
    /// <returns>Quest properties matching the id; null otherwise.</returns>
    public QuestProperties Get(int questId) => _quests.TryGetValue(questId, out QuestProperties quest) ? quest : null;

    /// <summary>
    /// Gets the quest properties based on the given quest identifier.
    /// </summary>
    /// <remarks>
    /// The identifier can either be the quest id or the full text identifier.
    /// </remarks>
    /// <param name="questIdentifier">Quest identifier.</param>
    /// <returns>Quest properties matching the identifier; null otherwise.</returns>
    public QuestProperties Get(string questIdentifier)
    {
        if (int.TryParse(questIdentifier, out int questId))
        {
            return Get(questId);
        }
        else
        {
            return _questByIdentifiers.TryGetValue(questIdentifier, out QuestProperties quest) ? quest : null;
        }
    }

    /// <summary>
    /// Gets a collection of <see cref="QuestProperties"/> matching the given predicate.
    /// </summary>
    /// <param name="predicate">Quest predicate.</param>
    /// <returns>Collection of <see cref="QuestProperties"/>.</returns>
    public IEnumerable<QuestProperties> Where(Func<QuestProperties, bool> predicate) => _quests.Values.Where(predicate);

    /// <summary>
    /// Loads the quests.
    /// </summary>
    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();

        IEnumerable<string> questFilePaths = Directory.GetFiles(GameResourcePaths.QuestsPath);

        if (questFilePaths.Any())
        {
            foreach (string questFilePath in questFilePaths)
            {
                string questIdentifier = Path.GetFileNameWithoutExtension(questFilePath);

                if (!TryGetQuestId(questIdentifier, out var questId))
                {
                    _logger.LogWarning($"Cannot find quest id for quest: '{questIdentifier}'.");
                    continue;
                }

                Lua luaFile = new();
                luaFile.DoFile(questFilePath);

                if (luaFile[questIdentifier] is not LuaTable questTable)
                {
                    continue;
                }

                QuestProperties quest = new()
                {
                    Id = questId,
                    Name = questIdentifier,
                    Title = questTable.GetValue<string>("title"),
                    StartCharacter = questTable.GetValue<string>("character"),
                    EndCharacter = questTable.GetValue<string>("end_character") ?? questTable.GetValue<string>("character"),
                    StartRequirements = new QuestStartRequirementsProperties
                    {
                        PreviousQuestId = questTable["start_requirements"].ToLuaTable()?.GetValue<string>("previous_quest"),
                        MinLevel = questTable["start_requirements"].ToLuaTable()?.GetValue<int>("min_level") ?? 0,
                        MaxLevel = questTable["start_requirements"].ToLuaTable()?.GetValue<int>("max_level") ?? 0,
                        Jobs = questTable["start_requirements.job"].ToLuaTable()?.Values
                            .ToArray<string>()
                            .Select(x => (DefineJob.Job)Enum.Parse(typeof(DefineJob.Job), x))
                    },
                    QuestEndCondition = new QuestEndConditionProperties
                    {
                        Items = questTable["end_conditions.items"].ToLuaTable()?.Values.ToArray<LuaTable>().Select(lua => new QuestItemProperties
                        {
                            Id = lua.GetValue<string>("id"),
                            Quantity = lua.GetValue<int>("quantity"),
                            Sex = lua.GetValue<GenderType>("sex"),
                            Remove = lua.GetValue<bool>("remove")
                        }),
                        Monsters = questTable["end_conditions.monsters"].ToLuaTable()?.Values.ToArray<LuaTable>().Select(lua => new QuestMonsterProperties
                        {
                            Id = lua.GetValue<string>("id"),
                            Amount = lua.GetValue<int>("quantity")
                        }),
                        Patrols = questTable["end_conditions.patrols"].ToLuaTable()?.Values.ToArray<LuaTable>().Select(lua => new QuestPatrolProperties
                        {
                            MapId = lua.GetValue<string>("map"),
                            Left = lua.GetValue<int>("left"),
                            Top = lua.GetValue<int>("top"),
                            Right = lua.GetValue<int>("right"),
                            Bottom = lua.GetValue<int>("bottom")
                        })
                    },
                    Rewards = new QuestRewardProperties(questTable),
                    Drops = LoadQuestItemDrops(questTable["drops"].ToLuaTable()),
                    BeginDialogs = (questTable["dialogs.begin"] as LuaTable)?.Values.ToArray<string>() ?? Enumerable.Empty<string>(),
                    AcceptedDialogs = (questTable["dialogs.begin_yes"] as LuaTable)?.Values.ToArray<string>() ?? Enumerable.Empty<string>(),
                    DeclinedDialogs = (questTable["dialogs.begin_no"] as LuaTable)?.Values.ToArray<string>() ?? Enumerable.Empty<string>(),
                    CompletedDialogs = (questTable["dialogs.completed"] as LuaTable)?.Values.ToArray<string>() ?? Enumerable.Empty<string>(),
                    NotFinishedDialogs = (questTable["dialogs.not_finished"] as LuaTable)?.Values.ToArray<string>() ?? Enumerable.Empty<string>(),
                };

                _quests.TryAdd(quest.Id, quest);
                _questByIdentifiers.TryAdd(quest.Name, quest);
            }
        }

        watch.Stop();
        _logger.LogInformation($"{_quests.Count} quests loaded in {watch.ElapsedMilliseconds}ms.");
    }

    private bool TryGetQuestId(string questIdentifier, out int questId)
    {
        if (!_defines.TryGetValue(questIdentifier, out questId))
        {
            if (questIdentifier.StartsWith("QUEST_") && !int.TryParse(questIdentifier.Replace("QUEST_", ""), out questId))
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// Loads the quest item drops.
    /// </summary>
    /// <param name="table">Main lua table.</param>
    /// <returns>Collection of <see cref="QuestItemDrop"/> objects.</returns>
    private static IEnumerable<QuestItemDropProperties> LoadQuestItemDrops(LuaTable table)
    {
        List<QuestItemDropProperties> questItemDrops = new();

        if (table is not null)
        {
            IEnumerable<LuaTable> values = table.Values.ToArray<LuaTable>().AsEnumerable();

            foreach (LuaTable dropItem in values)
            {
                if (dropItem["item_id"] == null || dropItem["probability"] == null || (dropItem["monsters"] == null && dropItem["monster_id"] == null))
                {
                    continue;
                }

                IEnumerable<string> monsterIds;

                if (dropItem["monsters"] is LuaTable dropItemMonstersTable && dropItemMonstersTable.Values.Count > 0)
                {
                    monsterIds = dropItemMonstersTable.Values.ToArray<string>().AsEnumerable();
                }
                else
                {
                    monsterIds = new[]
                    {
                        dropItem.GetValue<string>("monster_id")
                    };
                }

                string itemId = dropItem.GetValue<string>("item_id");
                long probability = dropItem.GetValue<long>("probability");
                int quantity = dropItem.GetValueOrDefault<int>("quantity", 1);

                foreach (string monsterId in monsterIds)
                {
                    questItemDrops.Add(new QuestItemDropProperties
                    {
                        ItemId = itemId,
                        MonsterId = monsterId,
                        Probability = probability,
                        Quantity = quantity
                    });
                }
            }
        }

        return questItemDrops;
    }
}
