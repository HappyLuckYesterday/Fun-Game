using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Packets
{
    public interface ISkillPacketFactory
    {
        /// <summary>
        /// Sends the skill tree update to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        void SendSkillTreeUpdate(IPlayerEntity player);

        /// <summary>
        /// Sends the skill to use.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="target">Current target entity.</param>
        /// <param name="skill">Skill to use.</param>
        /// <param name="castingTime">Skill casting time.</param>
        /// <param name="skillUseType">Skill use type.</param>
        void SendUseSkill(ILivingEntity player, IWorldEntity target, Skill skill, int castingTime, SkillUseType skillUseType);

        /// <summary>
        /// Sends a request to cancel the current skill.
        /// </summary>
        /// <param name="player">Current player</param>
        void SendSkillCancellation(IPlayerEntity player);

        /// <summary>
        /// Sends the skill reset packet to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="skillPoints">Skill points.</param>
        void SendSkillReset(IPlayerEntity player, ushort skillPoints);

        /// <summary>
        /// Sends the skill state to the given entity.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        /// <param name="skillId">Skill id.</param>
        /// <param name="skillLevel">Skill level.</param>
        /// <param name="time">Skill remaining time.</param>
        void SendSkillState(ILivingEntity entity, int skillId, int skillLevel, int time);
    }
}
