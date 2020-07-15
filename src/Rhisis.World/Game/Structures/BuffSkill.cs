using Rhisis.Core.Data;
using System.Collections.Generic;

namespace Rhisis.World.Game.Structures
{
    public class BuffSkill : Buff
    {
        /// <summary>
        /// Gets the buff skill id.
        /// </summary>
        public int SkillId { get; }

        /// <summary>
        /// Gets the buff skill level.
        /// </summary>
        public int SkillLevel { get; }

        /// <summary>
        /// Creates a new <see cref="BuffSkill"/> instance.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <param name="skillLevel">Skill level.</param>
        /// <param name="remainingTime">Buff remaining time.</param>
        /// <param name="attributes">Bonus attributes.</param>
        public BuffSkill(int skillId, int skillLevel, int remainingTime, IDictionary<DefineAttributes, int> attributes)
            : base(remainingTime, attributes)
        {
            SkillId = skillId;
            SkillLevel = skillLevel;
        }

        public override bool Equals(object obj)
        {
            if (obj is BuffSkill buffSkill)
            {
                return SkillId == buffSkill.SkillId;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() => (int)Id;
    }
}