using HealthChecks.UI.Client;
using Kiota.MinimalApi.Helpers;
using Kiota.MinimalApi.Options;
using Kiota.MinimalApi.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<WebAppOptions>().Bind(builder.Configuration.GetSection("WebAppOptions"));
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<CategoryServiceInMemory>();
builder.Services.AddSingleton<ProjectServiceInMemory>();
builder.Services.AddTransient<ILogger>(p =>
{
    var loggerFactory = p.GetRequiredService<ILoggerFactory>();
    return loggerFactory.CreateLogger("Kiota.MinimalApi");
});
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Kiota Example API",
            Version = "v1",
            Description = "API for demo purposes to understand Kiota better.",
            Contact = new()
            {
                Name = "Kiota Demo App",
                Email = "kiota@thisiscool.com"
            }
        };
        var localServer = new OpenApiServer
        {
            Description = "Local server for development purposes",
            Url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://localhost:5010"
        };
        document.Servers.Add(localServer);
        var prodServer = new OpenApiServer
        {
            Description = "Production server",
            Url = Environment.GetEnvironmentVariable("PROD_URL") ?? "https://kiota.example.com"
        };
        document.Servers.Add(prodServer);
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.MapOpenApi();
// to view scalar data go to https://localhost:<port>/scalar/v1
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Kiota Example API")
        .WithDownloadButton(true)
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.MapGroup(RouteHelper.CategoryControllerRoute)
    .MapCategoriesApi()
    .WithOpenApi();

app.MapGroup(RouteHelper.ProjectControllerRoute)
    .MapProjectsApi()
    .WithOpenApi();

app.MapHealthChecks($"/{RouteHelper.HealthRoute}", new HealthCheckOptions
{
    Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();
app.UseForwardedHeaders();
app.Run();