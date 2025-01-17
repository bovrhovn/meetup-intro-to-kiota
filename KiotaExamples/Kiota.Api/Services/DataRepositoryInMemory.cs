using Microsoft.Extensions.Caching.Memory;

namespace Kiota.Api.Services;

public abstract class DataRepositoryInMemory<T>(string dataRepoName, IMemoryCache memoryCache)
    where T : class
{
    private List<T>? GetFromMemoryCache() => memoryCache.TryGetValue(dataRepoName, out List<T>? data) ? data : [];
    
    public abstract void Init();
    protected void SetData(List<T> data) => memoryCache.Set(dataRepoName, data);

    public List<T>? GetAll() => GetFromMemoryCache();
    public bool Add(T item)
    {
        var list = GetFromMemoryCache();
        list?.Add(item);
        memoryCache.Set(dataRepoName, list);
        return true;
    }
    public bool UpdateOrInsert(T item)
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
    public bool Delete(T item)
    {
        var list = GetFromMemoryCache();
        list?.Remove(item);
        memoryCache.Set(dataRepoName, list);
        return true;
    }
}