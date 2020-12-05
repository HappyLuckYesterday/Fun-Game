namespace Rhisis.Game.Abstractions.Caching
{
    /// <summary>
    /// Provides cache keys.
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// Gets the cache key for all connected world channels to a cluster.
        /// </summary>
        ClusterWorldChannels = 0,

        /// <summary>
        /// Gets the cache key for all players connected to a cluster.
        /// </summary>
        ClusterPlayers = 1,

        /// <summary>
        /// Gets the cache key for all guilds available for a cluster.
        /// </summary>
        ClusterGuilds = 2
    }
}