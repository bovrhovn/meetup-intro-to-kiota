using Microsoft.OpenApi.Readers;
using Spectre.Console;

AnsiConsole.WriteLine("Calling Open API to generate call to our own API");
var url = Environment.GetEnvironmentVariable("API_URL") ?? "https://localhost:5010";
AnsiConsole.MarkupLine($"[bold]API URL[/]: [link]{url}[/]");
var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Is this the correct URL to get OpenApi from?")
    { ShowChoices = false });
if (!confirmation)
{
    AnsiConsole.Write(new Markup("Please set the [red]API_URL[/] environment variable to the correct URL"));
    return;
}

var openApiSpec = await new HttpClient { BaseAddress = new Uri(url) }.GetStringAsync("openapi/v1.json");
//AnsiConsole.WriteLine("OpenAPI Spec: " + openApiSpec);
if (string.IsNullOrEmpty(openApiSpec))
{
    AnsiConsole.MarkupLine("Failed to [bold]retrieve[/] OpenAPI Spec");
    return;
}

var openApiDocument = new OpenApiStringReader().Read(openApiSpec, out var diagnostic);
if (diagnostic.Errors.Count > 0)
{
    AnsiConsole.MarkupLine("Failed to [bold]parse[/] OpenAPI Spec");
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