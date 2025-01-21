using System.ComponentModel;

namespace Kiota.MinimalApi.Models;

public class Category
{
    [Description("The unique identifier of the category")]
    public string CategoryId { get; init; } = Guid.NewGuid().ToString();
    [Description("The name of the category")]
    public required string Name { get; set; }
}
