using Rhisis.Abstractions.Caching;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Infrastructure.Caching
{
    internal sealed class RhisisCache<TObject> : IRhisisCache<TObject> where TObject : class, new()
    {
        private readonly ConcurrentDictionary<int, TObject> _cache = new();

        public void Clear() => _cache.Clear();

        public bool Contains(int key) => _cache.ContainsKey(key);

        public bool Delete(int key) => _cache.TryRemove(key, out _);

        public TObject Get(int key) => _cache.GetValueOrDefault(key, default);

        public bool Set(int key, TObject @object)
        {
            _cache[key] = @object;

            return true;
        }
    }
}
