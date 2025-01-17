using Bogus;
using Kiota.Api.Helpers;
using Kiota.Api.Models;
using Kiota.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Kiota.Api.Controllers;

[ApiController, Route(RouteHelper.DataControllerRoute)]
public class DataController(ILogger<DataController> logger, IOptions<WebAppOptions> webAppOptionsValue)
    : ControllerBase
{
    [HttpGet]
    [Route(RouteHelper.RandomRoute)]
    [EndpointSummary("This get random data for the model.")]
    [EndpointDescription("This get random data using library Bogus.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetRandomDataAsync()
    {
        logger.LogInformation("Called random data endpoint at {DateCalled} to get {NumberOfItems} results back",
            DateTime.UtcNow, webAppOptionsValue.Value.DataCount);
        var faker = new Faker<Project>()
            .Generate(webAppOptionsValue.Value.DataCount);
        logger.LogInformation("Returning {Count} random data by using bogus library", faker.Count);
        return Ok(faker.ToList());
    }

    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [EndpointSummary("This is a health check for the data controller.")]
    [EndpointDescription("This is a health check for the data controller to see if API is online.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive data endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName}" };
    }
}