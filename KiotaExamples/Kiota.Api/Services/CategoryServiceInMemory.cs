using Bogus;
using Kiota.Api.Models;
using Kiota.Api.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Kiota.Api.Services;

public class CategoryServiceInMemory(
    IMemoryCache cache,
    ILogger<CategoryServiceInMemory> logger,
    IOptions<WebAppOptions> webAppOptionsValue)
    : DataRepositoryInMemory<Category>("categories", cache, logger)
{
    public override void InitData()
    {
        var categories = new Faker<Category>()
            .RuleFor(x => x.CategoryId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .Generate(webAppOptionsValue.Value.DataCount);
        AddRange(categories.ToArray());
    }
}