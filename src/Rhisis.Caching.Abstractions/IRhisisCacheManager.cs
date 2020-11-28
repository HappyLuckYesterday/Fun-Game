namespace Rhisis.Caching.Abstractions
{
    public interface IRhisisCacheManager
    {
        IRhisisCache GetCache(int cacheId = -1);
    }
}
