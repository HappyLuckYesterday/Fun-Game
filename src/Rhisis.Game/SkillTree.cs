using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Sylver.Network.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game
{
    public class SkillTree : ISkillTree
    {
        private readonly IPlayer _player;
        private readonly List<ISkill> _skills;

        public ushort SkillPoints { get; }

        public SkillTree(IPlayer player, ushort skillPoints)
        {
            _player = player;
            _skills = new List<ISkill>();
            SkillPoints = skillPoints;
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
            int skillCount = _skills.Count();
            int otherSkillCount = (int)DefineJob.JobMax.MAX_SKILLS - skillCount;

            for (int i = 0; i < skillCount; i++)
            {
                _skills.ElementAt(i).Serialize(packet);
            }

            for (int i = 0; i < otherSkillCount; i++)
            {
                packet.Write(-1);
                packet.Write(0);
            }
        }

        public IEnumerator<ISkill> GetEnumerator() => _skills.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _skills.GetEnumerator();
    }
}
