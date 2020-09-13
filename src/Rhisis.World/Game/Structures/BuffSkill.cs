using Rhisis.Game.Common;
using Sylver.Network.Data;
using System.Collections.Generic;

namespace Rhisis.World.Game.Structures
{
    public class BuffSkill : Buff
    {
        public override BuffType Type => BuffType.Skill;

        /// <summary>
        /// Gets the buff skill database id.
        /// </summary>
        public int? DatabaseId { get; }

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
        public BuffSkill(int skillId, int skillLevel, int remainingTime, IDictionary<DefineAttributes, int> attributes, int? databaseId = null)
            : base(remainingTime, attributes)
        {
            SkillId = skillId;
            SkillLevel = skillLevel;
            DatabaseId = databaseId;
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

        public override void Serialize(INetPacketStream packet)
        {
            packet.Write((short)Type);
            packet.Write((short)SkillId);
            packet.Write(SkillLevel);
            packet.Write(RemainingTime);
        }
    }
}