using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using Rhisis.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class SkillTree : IEnumerable<Skill>
{
    /// <summary>
    /// Skill points usage based on the job type.
    /// </summary>
    public static readonly IReadOnlyDictionary<DefineJob.JobType, int> SkillPointUsage = new Dictionary<DefineJob.JobType, int>()
    {
        { DefineJob.JobType.JTYPE_BASE, 1 },
        { DefineJob.JobType.JTYPE_EXPERT, 2 },
        { DefineJob.JobType.JTYPE_PRO, 3 },
        { DefineJob.JobType.JTYPE_MASTER, 3 },
        { DefineJob.JobType.JTYPE_HERO, 3 }
    };

    private readonly Player _player;
    private readonly List<Skill> _skills = new();

    public SkillTree(Player player)
    {
        _player = player;
    }

    public void AddSkillPointsForLevelUp(int level, bool sendToPlayer = true)
    {
        int skillPointsToAdd = (level - 1) / 20 + 2;
        _player.AddSkillPoints((ushort)skillPointsToAdd, sendToPlayer);
    }

    public Skill GetSkill(int skillId) => _skills.FirstOrDefault(x => x.Id == skillId);

    public Skill GetSkillAtIndex(int skillIndex) => _skills.ElementAt(skillIndex);

    public bool TryGetSkillAtIndex(int skillIndex, out Skill skill)
    {
        skill = _skills.ElementAtOrDefault(skillIndex);

        return skill is not null;
    }

    public bool HasSkill(int skillId) => _skills.Any(x => x.Id == skillId);

    public void SetSkill(Skill skill)
    {
        if (skill is null)
        {
            throw new ArgumentNullException(nameof(skill));
        }

        Skill existingSkill = GetSkill(skill.Id);

        if (existingSkill is not null)
        {
            existingSkill.Level = skill.Level;
        }
        else
        {
            _skills.Add(skill);
        }
    }

    public void SetSkillLevel(int skillId, int level)
    {
        Skill skill = GetSkill(skillId);

        if (skill is not null)
        {
            skill.Level = level;
        }
    }

    public void SetSkills(IEnumerable<Skill> skills)
    {
        _skills.Clear();
        _skills.AddRange(skills);
    }

    public void Serialize(FFPacket packet)
    {
        var skillCount = _skills.Count;
        var otherSkillCount = (int)DefineJob.JobMax.MAX_SKILLS - skillCount;

        for (var i = 0; i < skillCount; i++)
        {
            _skills[i].Serialize(packet);
        }

        for (var i = 0; i < otherSkillCount; i++)
        {
            packet.WriteInt32(-1);
            packet.WriteInt32(0);
        }
    }

    public IEnumerator<Skill> GetEnumerator() => _skills.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _skills.GetEnumerator();

    public void Update(IReadOnlyDictionary<int, int> skillsToUpdate)
    {
        int requiredSkillPoints = 0;

        foreach (KeyValuePair<int, int> skillToUpdate in skillsToUpdate)
        {
            int skillId = skillToUpdate.Key;
            int skillLevel = skillToUpdate.Value;
            Skill skill = GetSkill(skillId);

            if (skill == null)
            {
                _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot find skill with id '{skillId}' for player '{_player}'.");
            }

            if (skill.Level == skillLevel)
            {
                // Skill hasn't change
                continue;
            }

            if (_player.Level < skill.Properties.RequiredLevel)
            {
                _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Player need to be level '{skill.Properties.RequiredLevel}' to learn this skill.");
            }

            if (!CheckRequiredSkill(skill.Properties.RequiredSkillId1, skill.Properties.RequiredSkillLevel1, skillsToUpdate))
            {
                SkillProperties requiredSkill1 = GameResources.Current.Skills.Get(skill.Properties.RequiredSkillId1);

                _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Skill '{requiredSkill1.Name}' must be at least Lv.{requiredSkill1.RequiredSkillLevel1}");
            }

            if (!CheckRequiredSkill(skill.Properties.RequiredSkillId2, skill.Properties.RequiredSkillLevel2, skillsToUpdate))
            {
                SkillProperties requiredSkill2 = GameResources.Current.Skills.Get(skill.Properties.RequiredSkillId2);

                _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Skill '{requiredSkill2.Name}' must be at least Lv.{requiredSkill2.RequiredSkillLevel1}");
            }

            if (skillLevel < 0 || skillLevel < skill.Level || skillLevel > skill.Properties.MaxLevel)
            {
                _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. The skill level is out of bounds.");
            }

            if (!SkillPointUsage.TryGetValue(skill.Properties.JobType, out int requiredSkillPointAmount))
            {
                _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Cannot find required skill point for job type '{skill.Properties.JobType}'.");
            }

            requiredSkillPoints += (skillLevel - skill.Level) * requiredSkillPointAmount;
        }

        if (_player.SkillPoints < requiredSkillPoints)
        {
            _player.SendDefinedText(DefineText.TID_RESKILLPOINT_ERROR);

            throw new InvalidOperationException($"Cannot update skills for player '{_player}'. Not enough skill points.");
        }

        _player.SkillPoints -= (ushort)requiredSkillPoints;

        foreach (KeyValuePair<int, int> skill in skillsToUpdate)
        {
            int skillId = skill.Key;
            int skillLevel = skill.Value;

            SetSkillLevel(skillId, skillLevel);
        }

        using DoUseSkillPointSnapshot skillTreeUpdateSnapshot = new(_player);

        _player.Send(skillTreeUpdateSnapshot);
    }

    /// <summary>
    /// Check if the required skill condition matches.
    /// </summary>
    /// <param name="requiredSkillId">Required skill id.</param>
    /// <param name="requiredSkillLevel">Required skill level.</param>
    /// <param name="skillsToUpdate">Dictionary of skills to update.</param>
    /// <returns>True if the requirement matches; false otherwise.</returns>
    private static bool CheckRequiredSkill(int requiredSkillId, int requiredSkillLevel, IReadOnlyDictionary<int, int> skillsToUpdate)
    {
        if (requiredSkillId == -1)
        {
            return true;
        }

        if (skillsToUpdate.TryGetValue(requiredSkillId, out int skillLevel))
        {
            return skillLevel >= requiredSkillLevel;
        }

        return false;
    }
}
