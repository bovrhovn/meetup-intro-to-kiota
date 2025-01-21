using Kiota.MinimalApi.Models;
using Kiota.MinimalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kiota.MinimalApi.Helpers;

public static class RouteHelper
{
    public const string HealthRoute = "health";
    public const string InitRoute = "init";
    public const string ProjectControllerRoute = "projects";
    public const string ProjectsByCategoryRoute = "by-category";
    public const string CategoryControllerRoute = "categories";
    public const string AllRoute = "all";

    public static RouteGroupBuilder MapCategoriesApi(this RouteGroupBuilder group)
    {
        group.MapGet($"/{AllRoute}", GetAllCategories)
            .Produces<List<Category>>()
            .WithSummary("Get all categories")
            .WithDescription("Get all categories available in the system")
            .WithName("GetAllCategories")
            .WithTags("categories", "get");

        group.MapPost($"/{InitRoute}", InitCategories)
            .WithSummary("Init random categories")
            .WithDescription("Init random categories with Bogus library")
            .WithName("InitCategories")
            .WithTags("categories", "init");
        return group;
    }

    private static IResult InitCategories([FromServices] CategoryServiceInMemory categoryService,
        [FromServices] ILogger logger)
    {
        logger.LogInformation("Setting random data for categories");
        categoryService.InitData();
        return Results.Ok();
    }

    private static IResult GetAllCategories([FromServices] CategoryServiceInMemory categoryService,
        [FromServices] ILogger logger)
    {
        logger.LogInformation("Getting all categories");
        return Results.Ok(categoryService.GetAll());
    }

    public static RouteGroupBuilder MapProjectsApi(this RouteGroupBuilder group)
    {
        group.MapGet($"/{AllRoute}", GetAllProjects)
            .Produces<List<Project>>()
            .WithSummary("Get all projects")
            .WithDescription("Get all projects available in the system")
            .WithName("GetAllProjects")
            .WithTags("projects", "get");
        group.MapGet($"/{ProjectsByCategoryRoute}/{{categoryId}}", GetProjectsByCategory)
            .Produces<List<Project>>()
            .WithSummary("Get projects by category")
            .WithDescription("Get all projects based on a specific category")
            .WithName("GetProjectsByCategory")
            .WithTags("projects", "bycategory");
        group.MapPost($"/{InitRoute}", InitProjects)
            .WithSummary("Init random projects")
            .WithDescription(
                "Init random projects with Bogus library and leveraging CategoryService to tied the categories to system")
            .WithName("InitProjects")
            .WithTags("projects", "init");
        return group;
    }

    private static IResult GetProjectsByCategory([FromServices] ProjectServiceInMemory projectServiceInMemory,
        [FromServices] ILogger logger, [FromRoute] string categoryId)
    {
        logger.LogInformation("Getting projects by category by {CategoryId}", categoryId);
        return Results.Ok(projectServiceInMemory.GetProjectsByCategory(categoryId));
    }

    private static IResult InitProjects([FromServices] ProjectServiceInMemory projectServiceInMemory,
        [FromServices] ILogger logger)
    {
        logger.LogInformation("Setting random data for projects");
        projectServiceInMemory.InitData();
        return Results.Ok();
    }

    private static IResult GetAllProjects([FromServices] ProjectServiceInMemory projectServiceInMemory,
        [FromServices] ILogger logger)
    {
        logger.LogInformation("Getting all projects");
        return Results.Ok(projectServiceInMemory.GetAll());
    }
}