using System.ComponentModel;

namespace Kiota.MinimalApi.Models;

public class Project
{
    [Description("The unique identifier of the project")]
    public string ProjectId { get; init; } = Guid.NewGuid().ToString();
    [Description("The name of the project")]
    public required string Name { get; set; }
    [Description("The description of the project")]
    public string Description { get; set; } = string.Empty;
    [Description("The created date of the project")]
    public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.UtcNow;
    [Description("The category of the project")]
    public required Category Category { get; set; }
}