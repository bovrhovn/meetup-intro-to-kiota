using Bogus;
using Kiota.Api.Models;
using Kiota.Api.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Kiota.Api.Services;

public class ProjectServiceInMemory(IMemoryCache cache, ILogger<ProjectServiceInMemory> logger,
    IOptions<WebAppOptions> webAppOptionsValue)
    : DataRepositoryInMemory<Project>("projects", cache, logger)
{
    public List<Project> GetProjectsByCategory(string categoryId)
    {
        logger.LogInformation("Getting projects by category");
        var projects = GetAll();
        return (projects ?? []).Where(x => x.Category.CategoryId == categoryId).ToList();
    }

    public override void InitData()
    {
        var projects = new Faker<Project>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Category, f => new Category
            {
                CategoryId = f.Random.Guid().ToString(),
                Name = f.Commerce.Categories(1).First()
            })
            .RuleFor(x => x.ProjectId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CreatedDate, f => f.Date.Past())
            .Generate(webAppOptionsValue.Value.DataCount);
        AddRange(projects.ToArray());
    }
}