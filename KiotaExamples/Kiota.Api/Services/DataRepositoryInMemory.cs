using Microsoft.Extensions.Caching.Memory;

namespace Kiota.Api.Services;

public abstract class DataRepositoryInMemory<T>(string dataRepoName, IMemoryCache memoryCache)
    where T : class
{
    protected List<T>? GetFromMemoryCache() => memoryCache.TryGetValue(dataRepoName, out List<T>? data) ? data : [];
    public virtual List<T>? GetAll() => GetFromMemoryCache();
    public virtual bool Add(T item)
    {
        var list = GetFromMemoryCache();
        list?.Add(item);
        memoryCache.Set(dataRepoName, list);
        return true;
    }
    public virtual bool UpdateOrInsert(T item)
    {
        var list = GetFromMemoryCache();
        if ((bool)list?.Contains(item))
        {
            list.Remove(item); //re-add item
            list.Add(item);
        }
        else
        {
            list.Add(item);
        }
        memoryCache.Set(dataRepoName, list);
        return true;
    }
    public virtual bool Delete(T item)
    {
        var list = GetFromMemoryCache();
        list?.Remove(item);
        memoryCache.Set(dataRepoName, list);
        return true;
    }
}