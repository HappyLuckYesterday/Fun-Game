using NLua;
using Rhisis.Core.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Resources.Properties.Quests;

public sealed class QuestRewardProperties
{
    private readonly int? _gold;
    private readonly int _minGold;
    private readonly int _maxGold;

    private readonly long? _experience;
    private readonly long _minExperience;
    private readonly long _maxExperience;

    private readonly DefineJob.Job? _rewardJob;

    private readonly LuaFunction _rewardJobFunction;
    private readonly LuaTable _mainLuaTable;

    /// <summary>
    /// Gets the quest minimum gold reward.
    /// </summary>
    public int MinGold { get; init; }

    /// <summary>
    /// Gets the quest maximum gold reward.
    /// </summary>
    public int MaxGold { get; init; }

    /// <summary>
    /// Gets the quest gold reward.
    /// </summary>
    public int Gold => _gold ?? FFRandom.Random(_minGold, _maxGold);

    /// <summary>
    /// Gets the quest experience reward.
    /// </summary>
    public long Experience => _experience ?? FFRandom.LongRandom(_minExperience, _maxExperience);

    /// <summary>
    /// Gets the quest items reward.
    /// </summary>
    public IEnumerable<QuestItemProperties> Items { get; init; }

    /// <summary>
    /// Gets a boolean value that indiciates if the system should restat the player.
    /// </summary>
    public bool Restat { get; init; }

    /// <summary>
    /// Gets a boolean value that indiciates if the systme should reskill the player.
    /// </summary>
    public bool Reskill { get; init; }

    /// <summary>
    /// Gets the number of skills points to offer to the player.
    /// </summary>
    public ushort SkillPoints { get; init; }

    public QuestRewardProperties(LuaTable mainLuaTable)
    {
        _mainLuaTable = mainLuaTable ?? throw new ArgumentNullException(nameof(mainLuaTable));

        if (mainLuaTable["rewards.gold"] is LuaTable goldRangeTable)
        {
            _minGold = goldRangeTable.GetValue<int>("min");
            _maxGold = goldRangeTable.GetValue<int>("max");
        }
        else
        {
            _gold = mainLuaTable["rewards"].ToLuaTable().GetValue<int>("gold");
        }

        if (mainLuaTable["rewards.exp"] is LuaTable experienceRangeTable)
        {
            _minExperience = experienceRangeTable.GetValue<long>("min");
            _maxExperience = experienceRangeTable.GetValue<long>("max");
        }
        else
        {
            _experience = mainLuaTable["rewards"].ToLuaTable().GetValue<int>("exp");
        }

        Items = mainLuaTable["rewards.items"].ToLuaTable()?.Values
            .ToArray<LuaTable>()
            .Select(lua => new QuestItemProperties
            {
                Id = lua.GetValue<string>("id"),
                Quantity = lua.GetValue<int>("quantity"),
                Sex = lua.GetValue<GenderType>("sex"),
                Refine = lua.GetValue<byte>("refine"),
                Element = lua.GetValue<ElementType>("element"),
                ElementRefine = lua.GetValue<byte>("element_refine")
            })
            .ToList();
        Restat = mainLuaTable["rewards"].ToLuaTable().GetValue<bool>("restat");
        Reskill = mainLuaTable["rewards"].ToLuaTable().GetValue<bool>("reskill");
        SkillPoints = mainLuaTable["rewards"].ToLuaTable().GetValue<ushort>("skill_points");

        _rewardJob = mainLuaTable["rewards"].ToLuaTable().GetValue<DefineJob.Job>("job");
        _rewardJobFunction = mainLuaTable["change_job_reward"].ToLuaFunction();
    }

    /// <summary>
    /// Gets the reward job based on the player information.
    /// </summary>
    /// <param name="player">Current player entity information.</param>
    /// <returns></returns>
    public DefineJob.Job GetJob(object player)
    {
        if (_rewardJobFunction is not null)
        {
            return (DefineJob.Job)Enum.Parse(typeof(DefineJob.Job), _rewardJobFunction.Call(_mainLuaTable, player).First()?.ToString());
        }

        return _rewardJob.GetValueOrDefault();
    }

    /// <summary>
    /// Checks if the quest has a job reward.
    /// </summary>
    /// <returns>True if the quest has job reward; false otherwise.</returns>
    public bool HasJobReward() => _rewardJobFunction != null || _rewardJob.HasValue;
}
