using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechanism to manage every feature related with moves.
    /// </summary>
    public interface IMobilitySystem
    {
        /// <summary>
        /// Calculates the movable entities positions in real-time.
        /// </summary>
        /// <param name="moverEntity">Moving entity.</param>
        void Execute(IMover moverEntity);
    }
}
