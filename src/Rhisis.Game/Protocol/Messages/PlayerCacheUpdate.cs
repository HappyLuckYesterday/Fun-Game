namespace Rhisis.Game.Protocol.Messages
{
    /// <summary>
    /// Provides a data structure that notifies when a player cache has been updated.
    /// </summary>
    public class PlayerCacheUpdate
    {
        /// <summary>
        /// Gets or sets the player id that represents the player cache that has been updated.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerCacheUpdate"/> instance.
        /// </summary>
        public PlayerCacheUpdate()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PlayerCacheUpdate"/> instance.
        /// </summary>
        /// <param name="playerId">Player id</param>
        public PlayerCacheUpdate(int playerId)
        {
            PlayerId = playerId;
        }
    }
}
