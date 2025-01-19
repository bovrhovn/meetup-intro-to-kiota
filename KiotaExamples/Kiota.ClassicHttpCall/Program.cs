using System.Text.Json;

Console.WriteLine("Calling HTTP classical way!");
var client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:5008");
var response = await client.GetAsync("categories/all");
response.EnsureSuccessStatusCode();
Console.WriteLine("Receiving response....");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine("Found the following >>>> ");
if (!string.IsNullOrEmpty(content))
{
    //deserialize the content to list of category records
    var categories = JsonSerializer.Deserialize<List<CategoryModel>>(content);
    ArgumentNullException.ThrowIfNull(categories);
    foreach (var category in categories)
    {
        Console.WriteLine($"Category Id: {category.CategoryId}, Name: {category.Name}");
    }
}
else
{
    Console.WriteLine("No results found");
}

record CategoryModel(string CategoryId, string Name);