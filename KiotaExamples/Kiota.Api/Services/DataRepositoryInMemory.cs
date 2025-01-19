using Microsoft.Extensions.Caching.Memory;

namespace Kiota.Api.Services;

public abstract class DataRepositoryInMemory<T>(string dataRepoName, 
    IMemoryCache memoryCache, ILogger logger)
    where T : class
{
    protected List<T>? GetFromMemoryCache() => 
        memoryCache.TryGetValue(dataRepoName, out List<T>? data) ? data : [];

    public abstract void InitData();
    
    public List<T>? GetAll() => GetFromMemoryCache();

    public bool Add(T item)
    {
        var list = GetFromMemoryCache();
        logger.LogInformation("Adding item to the cache");
        list?.Add(item);
        logger.LogInformation("Setting memory");
        memoryCache.Set(dataRepoName, list);
        return true;
    }

    protected void AddRange(params T[] items)
    {
        var list = GetFromMemoryCache();
        if (list is { Count: 0 })
        {
            logger.LogInformation("Adding items to the cache");
            list.AddRange(items);
            memoryCache.Set(dataRepoName, list);
        }
    }
    
    public bool UpdateOrInsert(T item)
    {
        var list = GetFromMemoryCache();
        ArgumentNullException.ThrowIfNull(list);
        if (list.Contains(item))
        {
            logger.LogInformation("Removing item from the cache and re-adding it");
            list.Remove(item); //re-add item
            list.Add(item);
        }
        else
        {
            logger.LogInformation("Adding item to the cache");
            list.Add(item);
        }

        memoryCache.Set(dataRepoName, list);
        return true;
    }

    public bool Delete(T item)
    {
        var list = GetFromMemoryCache();
        logger.LogInformation("Removing item from the cache");
        list?.Remove(item);
        memoryCache.Set(dataRepoName, list);
        return true;
    }
}