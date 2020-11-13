using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechanism to manage the special effects
    /// </summary>
    public interface ISpecialEffectSystem
    {
        /// <summary>
        /// Starts a new special effect.
        /// </summary>
        /// <param name="worldObject">World object that activates the special effect.</param>
        /// <param name="specialEffect">Special effect to start.</param>
        /// <param name="followObject">Boolean value that indicates if the special effect should follow the given world object.</param>
        void StartSpecialEffect(IWorldObject worldObject, DefineSpecialEffects specialEffect, bool followObject = true);

        /// <summary>
        /// Sets the player's state mode base motion.
        /// </summary>
        /// <param name="entity">World object that activates or deactivates the state mode.</param>
        /// <param name="motionState"></param>
        /// <param name="item"></param>
        void SetStateModeBaseMotion(IWorldObject worldObject, StateModeBaseMotion motionState, IItem item = null);
    }
}
