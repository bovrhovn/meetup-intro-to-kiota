using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Spectre.Console;

AnsiConsole.WriteLine("Simple Api call to get projects and categories");
// run kiota to get the projects and categories
var url = Environment.GetEnvironmentVariable("API_URL") ?? "https://localhost:5010";
AnsiConsole.MarkupLine($"[bold]API URL[/]: [link]{url}[/]");
var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Is this the correct URL to get OpenApi from?")
    { ShowChoices = false });
if (!confirmation)
{
    AnsiConsole.Write("Please set the [red]API_URL[/] environment variable to the correct URL");
    return;
}

// open terminal and run the kiota client
// add dependencies to the project - Microsoft.Kiota.Bundle
// generate client from openapi spec provided by the minimal api
// kiota generate -d https://localhost:5010/openapi/v1.json -o ./Services -n Kiota.RestApi -l CSharp -c KiotaMinimalApiClient --skip-generation-if-type-exists
// run the client to get the categories
var authenticationProvider = new AnonymousAuthenticationProvider();
var client = new Kiota.RestApi.KiotaMinimalApiClient(new HttpClientRequestAdapter(authenticationProvider));
var categoriesTable = new Table();
categoriesTable.AddColumn("Category Id");
categoriesTable.AddColumn("Name");
//load temp data
await client.Categories.Init.PostAsync();
var categories = await client.Categories.All.GetAsync();
if (categories == null)
{
    AnsiConsole.WriteLine("Failed to retrieve categories");
    return;
}
categories.ForEach(currentCategory => 
    categoriesTable.AddRow(currentCategory.CategoryId!, currentCategory.Name!));
AnsiConsole.Write(categoriesTable);
