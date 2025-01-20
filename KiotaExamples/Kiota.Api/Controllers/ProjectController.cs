using Bogus;
using Kiota.Api.Helpers;
using Kiota.Api.Models;
using Kiota.Api.Options;
using Kiota.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Kiota.Api.Controllers;

[ApiController, Route(RouteHelper.ProjectControllerRoute)]
public class ProjectController(
    ILogger<ProjectController> logger,
    IOptions<WebAppOptions> webAppOptionsValue,
    ProjectServiceInMemory projectServiceInMemory,
    CategoryServiceInMemory categoryServiceInMemory)
    : ControllerBase
{
    [HttpPost]
    [Route(RouteHelper.InitRoute)]
    [EndpointSummary("This inits projects in memory")]
    [EndpointDescription("This get random data using library Bogus in memory implementation for random projects.")]
    [EndpointGroupName("Projects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult InitProjects()
    {
        logger.LogInformation("Called init projects endpoint at {DateCalled}", DateTime.UtcNow);
        projectServiceInMemory.InitData();
        return Ok();
    }

    [HttpGet]
    [Route(RouteHelper.RandomRoute)]
    [EndpointSummary("This get random project data for the model.")]
    [EndpointDescription("This get random data using library Bogus to get projects based on provided data.")]
    [EndpointGroupName("Projects")]
    [Produces(typeof(List<Project>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetRandomDataAsync()
    {
        logger.LogInformation("Called random data endpoint at {DateCalled} to get {NumberOfItems} results back",
            DateTime.UtcNow, webAppOptionsValue.Value.DataCount);
        categoryServiceInMemory.InitData(); //if data is not initialized
        var categories = categoryServiceInMemory.GetAll();
        var projects = new Faker<Project>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Category, f => f.PickRandom(categories))
            .RuleFor(x => x.ProjectId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CreatedDate, f => f.Date.Past())
            .Generate(webAppOptionsValue.Value.DataCount);
        logger.LogInformation("Returning {Count} random project data by using bogus library", projects.Count);
        return Ok(projects.ToList());
    }

    [HttpGet]
    [Route(RouteHelper.ProjectsByCategoryRoute + "/{categoryId}")]
    [EndpointSummary("This get projects based on category id.")]
    [EndpointDescription("This get project data using library Bogus based on provided category.")]
    [EndpointGroupName("Projects")]
    [Produces(typeof(List<Project>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProjectsByCategoryAsync(string categoryId)
    {
        logger.LogInformation(
            "Called get projects by category endpoint at {DateCalled} to get {CategoryId} results back",
            DateTime.UtcNow, categoryId);
        var projects = projectServiceInMemory.GetProjectsByCategory(categoryId);
        logger.LogInformation("Returning {Count} projects data based on category provided", projects);
        return Ok(projects);
    }

    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [EndpointSummary("This is a health check for the data controller.")]
    [EndpointDescription("This is a health check for the data controller to see if API is online.")]
    [EndpointGroupName("Health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive data endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName} for projects" };
    }
}