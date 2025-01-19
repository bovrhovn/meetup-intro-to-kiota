using System.Net;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Kiota.Api.Helpers;
using Kiota.Api.Options;
using Kiota.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<WebAppOptions>().Bind(builder.Configuration.GetSection("WebAppOptions"));
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

//adding services to the DI container
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CategoryServiceInMemory>();
builder.Services.AddSingleton<ProjectServiceInMemory>();

//configure the JSON serializer to ignore reference loops
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseRouting();
app.MapOpenApi();
app.MapControllers();
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            var message = $"{exception.Error.Message}";
            await context.Response.WriteAsync(message).ConfigureAwait(false);
        }
    });
});

app.MapHealthChecks($"/{RouteHelper.HealthRoute}", new HealthCheckOptions
{
    Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();
app.Run();