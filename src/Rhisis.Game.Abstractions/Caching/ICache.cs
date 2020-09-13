using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Caching
{
    public interface ICache<in TKey, TValue>
    {
        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="key">Unique key for the item</param>
        /// <param name="item">Actual item</param>
        /// <returns></returns>
        bool Add(TKey key, TValue item);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Unique to remove coupled item</param>
        /// <returns>true if successfully removed</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Try and get the item with given unique key
        /// </summary>
        /// <param name="key">Unique key to get the item with</param>
        /// <returns>Item or default value if not found</returns>
        TValue TryGetOrDefault(TKey key);

        ICollection<TValue> Items { get; }
    }
}