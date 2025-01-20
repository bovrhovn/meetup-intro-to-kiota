using System.Net;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Kiota.Api.Helpers;
using Kiota.Api.Options;
using Kiota.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<WebAppOptions>().Bind(builder.Configuration.GetSection("WebAppOptions"));
 builder.Services.Configure<ForwardedHeadersOptions>(options =>
     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Categories and Projects API",
            Version = "v1",
            Description = "API for demonstrating how Kiota works.",
            Contact = new()
            {
                Name = "Kiota Demos",
                Email = "bovrhovn@microsoft.com"
            }
        };
        return Task.CompletedTask;
    });
});
builder.Services.AddOpenApi("kiota-api");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

//adding services to the DI container
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CategoryServiceInMemory>();
builder.Services.AddSingleton<ProjectServiceInMemory>();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();
app.UseRouting();
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();
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