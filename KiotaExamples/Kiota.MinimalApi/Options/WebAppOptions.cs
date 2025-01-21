using System.ComponentModel;

namespace Kiota.MinimalApi.Options;

public sealed class WebAppOptions
{
    [Description("The number of data items to generate")]
    public int DataCount { get; set; } = 100;
}