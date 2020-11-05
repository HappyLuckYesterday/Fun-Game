using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IRegionTriggerSystem
    {
        /// <summary>
        /// Checks if the current player's position intersects a wrapzone.
        /// If it intersects then we teleport the player to the destination map.
        /// </summary>
        /// <param name="player">Current player.</param>
        void CheckWrapzones(IPlayer player);
    }
}
