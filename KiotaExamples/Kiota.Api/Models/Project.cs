namespace Kiota.Api.Models;

public class Project
{
    public string ProjectId { get; init; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.UtcNow;
    public required Category Category { get; set; }
}