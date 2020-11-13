using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechanism to calculate a mover's health.
    /// </summary>
    public interface IHealthFormulas
    {
        /// <summary>
        /// Gets the maximum origin HP of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Maximum Original HP.</returns>
        int GetMaxOriginHp(IMover entity);

        /// <summary>
        /// Gets the maximum origin MP of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Maximum Original MP.</returns>
        int GetMaxOriginMp(IMover entity);

        /// <summary>
        /// Gets the maximum origin FP of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Maximum Original FP.</returns>
        int GetMaxOriginFp(IMover entity);

        /// <summary>
        /// Gets the maximum HP of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Maximum HP.</returns>
        int GetMaxHp(IMover entity);

        /// <summary>
        /// Gets the maximum MP of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Maximum MP.</returns>
        int GetMaxMp(IMover entity);

        /// <summary>
        /// Gets the maximum FP of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Maximum FP.</returns>
        int GetMaxFp(IMover entity);

        /// <summary>
        /// Gets the HP recovery of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>HP recovery.</returns>
        int GetHpRecovery(IMover entity);

        /// <summary>
        /// Gets the MP recovery of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>MP recovery.</returns>
        int GetMpRecovery(IMover entity);

        /// <summary>
        /// Gets the FP recovery of a mover.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>FP recovery.</returns>

        int GetFpRecovery(IMover entity);
    }
}
