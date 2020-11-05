using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface ITeleportSystem
    {
        /// <summary>
        /// Teleports the player to the given position in the current map.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="position">Position to teleport.</param>
        /// <param name="sendToPlayer">Send the position to the player and others.</param>
        void Teleport(IPlayer player, Vector3 position, bool sendToPlayer = true);

        /// <summary>
        /// Teleports the player to the given position on the given map id.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="position">Position to teleport.</param>
        /// <param name="mapId">Target map id.</param>
        /// <param name="sendToPlayer">Send the position to the player and others.</param>
        void Teleport(IPlayer player, Vector3 position, int mapId, bool sendToPlayer);
    }
}
