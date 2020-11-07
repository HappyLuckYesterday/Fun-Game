using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    public interface ISkillTree : IPacketSerializer, IEnumerable<ISkill>
    {
        ushort SkillPoints { get; }

        void AddSkillPoints(ushort skillPointsToAdd, bool sendToPlayer = true);

        ISkill GetSkill(int skillId);

        ISkill GetSkillAtIndex(int skillIndex);

        bool HasSkill(int skillId);

        void Reskill();

        void SetSkillLevel(int skillId, int level);

        void SetSkills(IEnumerable<ISkill> skills);

        void Update(IReadOnlyDictionary<int, int> skillsToUpdate);
    }
}
