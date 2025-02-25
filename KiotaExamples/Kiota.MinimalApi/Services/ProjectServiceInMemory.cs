﻿using Bogus;
using Kiota.MinimalApi.Models;
using Kiota.MinimalApi.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Kiota.MinimalApi.Services;

public class ProjectServiceInMemory(IMemoryCache cache, ILogger<ProjectServiceInMemory> logger,
    IOptions<WebAppOptions> webAppOptionsValue, CategoryServiceInMemory categoryServiceInMemory)
    : DataRepositoryInMemory<Project>("projects", cache, logger)
{
    public List<Project> GetProjectsByCategory(string categoryId)
    {
        logger.LogInformation("Getting projects by category {CategoryId}", categoryId);
        var projects = GetAll();
        return (projects ?? []).Where(x => x.AssociatedCategory.CategoryId == categoryId).ToList();
    }

    public override void InitData()
    {
        categoryServiceInMemory.InitData(); //if data is not initialized
        var categories = categoryServiceInMemory.GetAll();
        var projects = new Faker<Project>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.AssociatedCategory, f => f.PickRandom(categories))
            .RuleFor(x => x.ProjectId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CreatedDate, f => f.Date.Past())
            .Generate(webAppOptionsValue.Value.DataCount);
        AddRange(projects.ToArray());
    }
}