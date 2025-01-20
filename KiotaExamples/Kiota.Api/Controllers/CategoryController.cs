using Kiota.Api.Helpers;
using Kiota.Api.Models;
using Kiota.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kiota.Api.Controllers;

[ApiController, Route(RouteHelper.CategoryControllerRoute)]
public class CategoryController(ILogger<CategoryController> logger, 
    CategoryServiceInMemory categoryServiceInMemory) : ControllerBase
{
    [HttpGet]
    [Route(RouteHelper.AllRoute)]
    [EndpointSummary("This gets all categories")]
    [EndpointDescription("This is gets all categories from memory and returns that to user.")]
    [EndpointGroupName("Categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(typeof(List<Category>))]
    public IActionResult GetAllCategories()
    {
        logger.LogInformation("Called get all categories endpoint at {DateCalled}", DateTime.UtcNow);
        var list = categoryServiceInMemory.GetAll();
        logger.LogInformation("Returning {Count} categories", list?.Count);
        return Ok(list);
    }
    
    [HttpPost]
    [Route(RouteHelper.InitRoute)]
    [EndpointSummary("This inits categories in memory")]
    [EndpointDescription("This get random data using library Bogus in memory implementation for random categories.")]
    [EndpointGroupName("Categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult InitProjects()
    {
        logger.LogInformation("Called init categories endpoint at {DateCalled}", DateTime.UtcNow);
        categoryServiceInMemory.InitData();
        return Ok();
    }
    
    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [EndpointSummary("This is a health check for the categories controller.")]
    [EndpointDescription("This is a health check for the categories controller to see if API is online.")]
    [EndpointGroupName("Health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive data endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName} for categories" };
    }
}