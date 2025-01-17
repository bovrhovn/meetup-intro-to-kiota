namespace Kiota.Api.Models;

public class Category
{
    public string CategoryId { get; init; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
}
