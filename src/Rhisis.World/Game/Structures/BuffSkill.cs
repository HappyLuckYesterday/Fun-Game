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
        public BuffSkill(int skillId, int skillLevel, int remainingTime)
            : base(remainingTime)
        {
            SkillId = skillId;
            SkillLevel = skillLevel;
        }
    }
}