using System.Text.Json;
using Spectre.Console;

AnsiConsole.WriteLine("Calling HTTP classical way!");
var url = Environment.GetEnvironmentVariable("API_URL") ?? "https://localhost:5010";
var client = new HttpClient();
client.BaseAddress = new Uri(url);
var choice = AnsiConsole.Prompt(new ConfirmationPrompt("Start with adding the data to the system?") { ShowChoices = false });
if (!choice)
{
    AnsiConsole.Write("Skipping data initialization, [red]exiting[/]...");
    return;
}

await client.PostAsync("categories/init", null);
var response = await client.GetAsync("categories/all");
response.EnsureSuccessStatusCode();
AnsiConsole.WriteLine("Receiving response....");
var content = await response.Content.ReadAsStringAsync();
AnsiConsole.WriteLine("Found the following data:");
if (!string.IsNullOrEmpty(content))
{
    var categories = JsonSerializer.Deserialize<List<CategoryModel>>(content);
    ArgumentNullException.ThrowIfNull(categories);
    var table = new Table();
    table.AddColumn("Category Id");
    table.AddColumn("Name", c => c.Centered());
    foreach (var category in categories) table.AddRow(category.categoryId, category.name);
    AnsiConsole.Write(table);
}
else
{
    AnsiConsole.Write("[red]No results[/] found");
}

internal record CategoryModel(string categoryId, string name);