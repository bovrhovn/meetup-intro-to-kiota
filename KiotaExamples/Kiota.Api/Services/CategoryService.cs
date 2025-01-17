using Bogus;
using Kiota.Api.Models;
using Kiota.Api.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Kiota.Api.Services;

public class CategoryService(IMemoryCache cache, IOptions<WebAppOptions> webAppOptionsValue)
    : DataRepositoryInMemory<Category>("categories", cache)
{
    public override void Init()
    {
        var categories = GetAll();
        ArgumentNullException.ThrowIfNull(categories);
        
        if (categories.Any()) return;
        
        var faker = new Faker<Category>()
            .RuleFor(name => name.Name, f => f.Commerce.Categories(1)[0])
            .Generate(webAppOptionsValue.Value.DataCount);
        categories.AddRange(faker);
        SetData(categories);
    }
}