using Kiota.Api.Helpers;
using Kiota.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kiota.Api.Controllers;

[ApiController, Route(RouteHelper.CategoryControllerRoute)]
public class CategoryController(ILogger<CategoryController> logger, CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [Route(RouteHelper.AllRoute)]
    [EndpointSummary("This gets all categories")]
    [EndpointDescription("This is gets all categories from memory and returns that to user.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllCategories()
    {
        logger.LogInformation("Called get all categories endpoint at {DateCalled}", DateTime.UtcNow);
        var list = categoryService.GetAll();
        logger.LogInformation("Returning {Count} categories", list?.Count);
        return Ok(list);
    }
    
    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [EndpointSummary("This is a health check for the categories controller.")]
    [EndpointDescription("This is a health check for the categories controller to see if API is online.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive data endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName}" };
    }
}