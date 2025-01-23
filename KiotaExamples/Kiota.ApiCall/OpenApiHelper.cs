using System.Diagnostics;
using System.Reflection;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Spectre.Console;

namespace Kiota.ApiCall;

public class OpenApiHelper
{
    private async Task<OpenApiDocument> GetOpenApiDocumentAsync(string url = "https://localhost:5010")
    {
        var openApiSpec = await new HttpClient { BaseAddress = new Uri(url) }
            .GetStringAsync("openapi/v1.json");
        if (string.IsNullOrEmpty(openApiSpec))
        {
            AnsiConsole.MarkupLine("Failed to [red]retrieve[/] OpenAPI Spec");
            throw new Exception("Failed to parse OpenAPI Spec");
        }

        var openApiDocument = new OpenApiStringReader().Read(openApiSpec, out var diagnostic);
        if (diagnostic.Errors.Count > 0)
        {
            AnsiConsole.MarkupLine("Failed to [red]parse[/] OpenAPI Spec");
            throw new Exception("Failed to parse OpenAPI Spec");
        }

        return openApiDocument;
    }

    public async Task GenerateApiClientCalls(string dirPath =
        @"D:\Projects\meetup-intro-to-kiota\KiotaExamples\Kiota.ApiCall\")
    {
        AnsiConsole.WriteLine("Generating Api Client and calls");
        var processStart = new ProcessStartInfo
        {
            FileName = "pwsh",
            Arguments = "call-kiota.ps1",
            WorkingDirectory = dirPath,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        var process = Process.Start(processStart);
        if (process == null)
        {
            AnsiConsole.MarkupLine("Failed to [red]start[/] process");
            return;
        }

        await process.WaitForExitAsync();
        var endOfOperation = await process.StandardOutput.ReadToEndAsync();
        AnsiConsole.WriteLine(endOfOperation);
    }

    public async Task OutputInfoAsync(string url = "https://localhost:5010")
    {
        var openApiDocument = await GetOpenApiDocumentAsync(url);
        //output data
        var table = new Table();
        table.AddColumn("Title");
        table.AddColumn(new TableColumn("Api paths").Centered());
        table.AddColumn(new TableColumn("Components").Centered());
        var pathKeys = "Paths:" + Environment.NewLine;
        openApiDocument.Paths.Keys.ToList()
            .ForEach(currentPath => pathKeys += currentPath + Environment.NewLine);
        var componentKeys = "Components:" + Environment.NewLine;
        openApiDocument.Components.Schemas.Keys.ToList()
            .ForEach(currentComponent => componentKeys += currentComponent + Environment.NewLine);
        table.AddRow(openApiDocument.Info.Title, pathKeys, componentKeys);
        AnsiConsole.Write(table);
    }

    public async Task GetAllCategoriesAsync(string url = "https://localhost:5010",
        string path = "Categories/All")
    {
        var openApiDocument = await GetOpenApiDocumentAsync(url);
        var categories = openApiDocument.Paths.Where(currentPath => currentPath.Key.Contains(path.ToLower()))
            .ToList();
        if (categories.Count == 0)
        {
            AnsiConsole.MarkupLine("No [red]categories[/] found");
            return;
        }

        var category = categories[0].Value.Operations.First();
        var categoriesFlow = path.Split('/');
        AnsiConsole.WriteLine($"Getting {category.Value.Summary} with methods {category.Key}");
        var currentAssembly = Assembly.GetExecutingAssembly()
            .GetType("Kiota.RestApi.ApiClient");
        ArgumentNullException.ThrowIfNull(currentAssembly);
        var categoriesProperty = currentAssembly.GetProperty(categoriesFlow[0]);
        ArgumentNullException.ThrowIfNull(categoriesProperty);
        var allProperty = categoriesProperty.PropertyType.GetProperty(categoriesFlow[1]);
        ArgumentNullException.ThrowIfNull(allProperty);
        AnsiConsole.WriteLine("Property name:" + allProperty.Name);
        var methodGet = allProperty.PropertyType.GetMethod($"{category.Key}Async");
        ArgumentNullException.ThrowIfNull(methodGet);
        AnsiConsole.WriteLine($"Method name: {methodGet.Name} return type:{methodGet.ReturnType}");
        var clientRequestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        var client = Activator.CreateInstance(currentAssembly, clientRequestAdapter);
        ArgumentNullException.ThrowIfNull(client);
        var categoriesPropertyValue = categoriesProperty.GetValue(client);
        ArgumentNullException.ThrowIfNull(categoriesPropertyValue);
        var allPropertyValue = allProperty.GetValue(categoriesPropertyValue);
        ArgumentNullException.ThrowIfNull(allPropertyValue);
        //dependencies abstractions
        var actionParameters = default(Action<RequestConfiguration<DefaultQueryParameters>>);
        var parameters = new object[] { actionParameters!, CancellationToken.None };
        var result = methodGet.Invoke(allPropertyValue, parameters) as Task;
        ArgumentNullException.ThrowIfNull(result);
        await result;
        var resultProperty = result.GetType().GetProperty("Result");
        ArgumentNullException.ThrowIfNull(resultProperty);        
        AnsiConsole.WriteLine(resultProperty.ToString() ?? string.Empty);
    }
}