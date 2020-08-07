using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Statistics
{
    public interface IStatisticsSystem
    {
        /// <summary>
        /// Updates the player's statistics.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="strength">Strength.</param>
        /// <param name="stamina">Stamina.</param>
        /// <param name="dexterity">Dexterity.</param>
        /// <param name="intelligence">Intelligence.</param>
        void UpdateStatistics(IPlayerEntity player, ushort strength, ushort stamina, ushort dexterity, ushort intelligence);

        /// <summary>
        /// Resets player statistics.
        /// </summary>
        /// <param name="player">Current player.</param>
        void Restat(IPlayerEntity player);

        /// <summary>
        /// Gets the entity's base strength.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity base strength.</returns>
        int GetBaseStrength(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's attribute strength.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity bonus strength.</returns>
        int GetAttributedStrength(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's total strength.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity total strength.</returns>
        int GetTotalStrength(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's base stamina.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity base stamina.</returns>
        int GetBaseStamina(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's attribute stamina.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity bonus stamina.</returns>
        int GetAttributedStamina(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's total stamina.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity total stamina.</returns>
        int GetTotalStamina(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's base dexterity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity base dexterity.</returns>
        int GetBaseDexterity(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's attribute dexterity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity bonus dexterity.</returns>
        int GetAttributedDexterity(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's total dexterity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity total dexterity.</returns>
        int GetTotalDexterity(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's base intelligence.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity base intelligence.</returns>
        int GetBaseIntelligence(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's attribute intelligence.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Entity bonus intelligence.</returns>
        int GetAttributedIntelligence(ILivingEntity entity);

        /// <summary>
        /// Gets the entity's total intelligence.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity total intelligence.</returns>
        int GetTotalIntelligence(ILivingEntity entity);
    }
}
