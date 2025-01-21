using HealthChecks.UI.Client;
using Kiota.MinimalApi.Helpers;
using Kiota.MinimalApi.Options;
using Kiota.MinimalApi.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<WebAppOptions>().Bind(builder.Configuration.GetSection("WebAppOptions"));
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<CategoryServiceInMemory>();
builder.Services.AddSingleton<ProjectServiceInMemory>();
    
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

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

