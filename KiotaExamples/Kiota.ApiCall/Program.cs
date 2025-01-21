using Microsoft.OpenApi.Readers;
using Spectre.Console;

AnsiConsole.WriteLine("Calling Open API to generate call to our own API");
var url = Environment.GetEnvironmentVariable("API_URL") ?? "https://localhost:5010";
AnsiConsole.MarkupLine($"[bold]API URL[/]: [link]{url}[/]");
var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Is this the correct URL to get OpenApi from?")
    { ShowChoices = false });
if (!confirmation)
{
    AnsiConsole.Write("Please set the [red]API_URL[/] environment variable to the correct URL");
    return;
}

var openApiSpec = await new HttpClient { BaseAddress = new Uri(url) }.GetStringAsync("openapi/kiota-api.json");
AnsiConsole.WriteLine("OpenAPI Spec: " + openApiSpec);
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
var pathKeys = "Paths:" + Environment.NewLine;
openApiDocument.Paths.Keys.ToList()
    .ForEach(currentPath => pathKeys += currentPath + Environment.NewLine);
table.AddRow(openApiDocument.Info.Title, pathKeys);
AnsiConsole.Write(table);