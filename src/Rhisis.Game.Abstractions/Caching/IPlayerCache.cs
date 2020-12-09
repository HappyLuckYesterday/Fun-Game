namespace Rhisis.Game.Abstractions.Caching
{
    /// <summary>
    /// Provides a mechanism to manage the player cache.
    /// </summary>
    public interface IPlayerCache
    {
        /// <summary>
        /// Gets the cached player by its player id.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        /// <returns>Cached player.</returns>
        CachedPlayer GetCachedPlayer(int playerId);

        /// <summary>
        /// Loads a player identified by the given player id and stores it into the cache.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <returns>Loaded cached player.</returns>
        CachedPlayer LoadCachedPlayer(int playerId);

        /// <summary>
        /// Sets the given player cache information into the cache.
        /// </summary>
        /// <param name="player">Player.</param>
        void SetCachedPlayer(CachedPlayer player);
    }
}
