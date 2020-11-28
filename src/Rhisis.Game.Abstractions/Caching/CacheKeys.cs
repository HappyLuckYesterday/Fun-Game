namespace Rhisis.Game.Abstractions.Caching
{
    /// <summary>
    /// Provides cache keys.
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// Gets the cache key for all connected world channels to a cluster.
        /// </summary>
        public const int ClusterWorldChannels = 0;

        /// <summary>
        /// Gets the cache key for all players connected to a cluster.
        /// </summary>
        public const int ClusterPlayers = 1;

        /// <summary>
        /// Gets the cache key for all guilds available for a cluster.
        /// </summary>
        public const int ClusterGuilds = 2;
    }
}