using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Infrastructure.Caching;

public sealed class RhisisCache<TKey, TObject>
    where TKey : struct, IEquatable<TKey>, IConvertible
    where TObject : class, new()
{
    private readonly ConcurrentDictionary<TKey, TObject> _cache = new();

    public void Clear() => _cache.Clear();

    public bool Contains(TKey key) => _cache.ContainsKey(key);

    public bool Delete(TKey key) => _cache.TryRemove(key, out _);

    public TObject Get(TKey key) => _cache.GetValueOrDefault(key, default);

    public bool Set(TKey key, TObject @object)
    {
        _cache[key] = @object;

        return true;
    }
}
