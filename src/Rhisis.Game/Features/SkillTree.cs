using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Protocol.Snapshots.Skills;
using Rhisis.Network.Snapshots;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class SkillTree : GameFeature, ISkillTree
    {
        private readonly IPlayer _player;
        private readonly IGameResources _gameResources;
        private readonly List<ISkill> _skills;

        public ushort SkillPoints { get; private set; }

        public SkillTree(IPlayer player, ushort skillPoints, IGameResources gameResources)
        {
            _player = player;
            _skills = new List<ISkill>();
            SkillPoints = skillPoints;
            _gameResources = gameResources;
        }

        public void AddSkillPoints(ushort skillPointsToAdd, bool sendToPlayer = true)
        {
            SkillPoints += skillPointsToAdd;

            if (sendToPlayer)
            {
                using var snapshot = new SetExperienceSnapshot(_player);
                _player.Send(snapshot);
            }
        }

        public ISkill GetSkill(int skillId) => _skills.FirstOrDefault(x => x.Id == skillId);

        public ISkill GetSkillAtIndex(int skillIndex) => _skills.ElementAtOrDefault(skillIndex);

        public bool HasSkill(int skillId) => _skills.Any(x => x.Id == skillId);

        public void SetSkillLevel(int skillId, int level)
        {
            ISkill skill = GetSkill(skillId);

            if (skill != null)
            {
                skill.Level = level;
            }
        }

        public void SetSkills(IEnumerable<ISkill> skills)
        {
            _skills.Clear();
            _skills.AddRange(skills);
        }

        public void Reskill()
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(INetPacketStream packet)
        {
            var skillCount = _skills.Count();
            var otherSkillCount = (int)DefineJob.JobMax.MAX_SKILLS - skillCount;

            for (var i = 0; i < skillCount; i++)
            {
                _skills.ElementAt(i).Serialize(packet);
            }

            for (var i = 0; i < otherSkillCount; i++)
            {
                packet.Write(-1);
                packet.Write(0);
            }
        }

        public IEnumerator<ISkill> GetEnumerator() => _skills.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _skills.GetEnumerator();

        public void Update(IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            int requiredSkillPoints = 0;

            foreach (KeyValuePair<int, int> skillToUpdate in skillsToUpdate)
            {
                int skillId = skillToUpdate.Key;
                int skillLevel = skillToUpdate.Value;
                ISkill skill = GetSkill(skillId);

                if (skill == null)
                {
                    SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                    throw new InvalidOperationException($"Cannot find skill with id '{skillId}' for player '{_player}'.");
                }

                if (skill.Level == skillLevel)
                {
                    // Skill hasn't change
                    continue;
                }

                if (_player.Level < skill.Data.RequiredLevel)
                {
                    SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                    throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Player need to be level '{skill.Data.RequiredLevel}' to learn this skill.");
                }

                if (!CheckRequiredSkill(skill.Data.RequiredSkillId1, skill.Data.RequiredSkillLevel1, skillsToUpdate))
                {
                    SkillData requiredSkill1 = _gameResources.Skills[skill.Data.RequiredSkillId1];

                    SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                    throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Skill '{requiredSkill1.Name}' must be at least Lv.{requiredSkill1.RequiredSkillLevel1}");
                }

                if (!CheckRequiredSkill(skill.Data.RequiredSkillId2, skill.Data.RequiredSkillLevel2, skillsToUpdate))
                {
                    SkillData requiredSkill2 = _gameResources.Skills[skill.Data.RequiredSkillId2];

                    SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                    throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Skill '{requiredSkill2.Name}' must be at least Lv.{requiredSkill2.RequiredSkillLevel1}");
                }

                if (skillLevel < 0 || skillLevel < skill.Level || skillLevel > skill.Data.MaxLevel)
                {
                    SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                    throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. The skill level is out of bounds.");
                }

                if (!GameConstants.SkillPointUsage.TryGetValue(skill.Data.JobType, out int requiredSkillPointAmount))
                {
                    SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                    throw new InvalidOperationException($"Cannot update skill with '{skillId}' for player '{_player}'. Cannot find required skill point for job type '{skill.Data.JobType}'.");
                }

                requiredSkillPoints += (skillLevel - skill.Level) * requiredSkillPointAmount;
            }

            if (SkillPoints < requiredSkillPoints)
            {
                SendDefinedText(_player, DefineText.TID_RESKILLPOINT_ERROR);

                throw new InvalidOperationException($"Cannot update skills for player '{_player}'. Not enough skill points.");
            }

            SkillPoints -= (ushort)requiredSkillPoints;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;

                SetSkillLevel(skillId, skillLevel);
            }

            using var skillTreeUpdateSnapshot = new DoUseSkillPointSnapshot(_player);

            _player.Send(skillTreeUpdateSnapshot);
        }

        /// <summary>
        /// Check if the required skill condition matches.
        /// </summary>
        /// <param name="requiredSkillId">Required skill id.</param>
        /// <param name="requiredSkillLevel">Required skill level.</param>
        /// <param name="skillsToUpdate">Dictionary of skills to update.</param>
        /// <returns>True if the requirement matches; false otherwise.</returns>
        private bool CheckRequiredSkill(int requiredSkillId, int requiredSkillLevel, IReadOnlyDictionary<int, int> skillsToUpdate)
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
}
