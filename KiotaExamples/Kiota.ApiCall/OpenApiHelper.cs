using Microsoft.OpenApi.Readers;
using Spectre.Console;

namespace Kiota.ApiCall;

public class OpenApiHelper
{
    public async Task OutputInfo(string url = "https://localhost:5010")
    {
        var openApiSpec = await new HttpClient { BaseAddress = new Uri(url) }
            .GetStringAsync("openapi/v1.json");
        //AnsiConsole.WriteLine("OpenAPI Spec: " + openApiSpec);
        if (string.IsNullOrEmpty(openApiSpec))
        {
            Console.WriteLine("Failed to retrieve OpenAPI Spec");
            return;
        }

        var openApiDocument = new OpenApiStringReader().Read(openApiSpec, out var diagnostic);
        if (diagnostic.Errors.Count > 0)
        {
            Console.WriteLine("Failed to parse OpenAPI Spec");
            return;
        }

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
}