using Kiota.ApiCall;
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

var openApiHelper = new OpenApiHelper();
await openApiHelper.OutputInfoAsync(url);

var operationOptions = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("What do you want to do?")
        .PageSize(3)
        .AddChoices("Generate classes based on OpenAPI", "Generate client and do calls"));

//generate the client and calls
if (operationOptions == "Generate classes based on OpenAPI")
    await openApiHelper.GenerateApiClientCalls();
else
    await openApiHelper.GetAllCategoriesAsync();