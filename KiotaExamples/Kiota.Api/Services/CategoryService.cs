using Kiota.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Kiota.Api.Services;

public class CategoryService(IMemoryCache cache)
    : DataRepositoryInMemory<Category>("categories", cache)
{
    
}