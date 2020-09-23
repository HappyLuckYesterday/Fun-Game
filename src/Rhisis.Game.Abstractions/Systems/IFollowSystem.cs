using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IFollowSystem
    {
        /// <summary>
        /// Follows a target entity.
        /// </summary>
        /// <param name="moverEntity">Current living entity.</param>
        /// <param name="targetMoverEntity">Target entity to follow.</param>
        /// <param name="distance">Distance.</param>
        void Follow(IMover moverEntity, IWorldObject targetMoverEntity, float distance = 1f);

        /// <summary>
        /// Follows a target mover entity and finds it by its id.
        /// </summary>
        /// <param name="moverEntity">Current mover entity.</param>
        /// <param name="targetId">Target mover entity id to follow.</param>
        /// <param name="distance">Distance.</param>
        void Follow(IMover moverEntity, uint targetId, float distance = 1f);

        /// <summary>
        /// Clears the follow target and stops following.
        /// </summary>
        /// <param name="moverEntity"></param>
        void Unfollow(IMover moverEntity);
    }
}
