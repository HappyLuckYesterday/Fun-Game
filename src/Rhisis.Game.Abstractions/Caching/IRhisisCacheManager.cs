namespace Rhisis.Game.Abstractions.Caching
{
    /// <summary>
    /// Provides a mechanism to access a rhisis cache.
    /// </summary>
    public interface IRhisisCacheManager
    {
        /// <summary>
        /// Gets a Rhisis cache by the given type.
        /// </summary>
        /// <param name="type">Cache type.</param>
        /// <returns></returns>
        IRhisisCache GetCache(CacheType type);

        /// <summary>
        /// Clears all caches.
        /// </summary>
        void ClearAllCaches();

        /// <summary>
        /// Clears a cache identified by the given cache key.
        /// </summary>
        /// <param name="type">Cache type.</param>
        void ClearCache(CacheType type);
    }
}
