namespace Rhisis.Game.Abstractions.Caching
{
    /// <summary>
    /// Provides a mechanism to manage a key-value cache.
    /// </summary>
    public interface IRhisisCache
    {
        /// <summary>
        /// Check if the the given key exists in the cache.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>True if the key contains a cached value; false otherwise.</returns>
        bool Contains(string key);

        /// <summary>
        /// Clears the cache data.
        /// </summary>
        void Clear();

        /// <summary>
        /// Deletes an object from the cache.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>True if deleted; false otherwise.</returns>
        bool Delete(string key);

        /// <summary>
        /// Gets a <typeparamref name="TObject"/> stored at the given key.
        /// </summary>
        /// <typeparam name="TObject">Object type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <returns>The stored object; null otherwise.</returns>
        TObject Get<TObject>(string key) where TObject : class, new();

        /// <summary>
        /// Stores a <typeparamref name="TObject"/> at the given key into the cache.
        /// </summary>
        /// <typeparam name="TObject">Object type.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="object">Object to store.</param>
        /// <returns>True if set into cache; false otherwise.</returns>
        bool Set<TObject>(string key, TObject @object);
    }
}
