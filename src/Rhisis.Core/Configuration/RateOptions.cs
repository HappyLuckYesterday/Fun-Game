using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class RateOptions
{
    /// <summary>
    /// Gets or sets the gold drop rate.
    /// </summary>
    [ConfigurationKeyName("gold")]
    public int Gold { get; set; }

    /// <summary>
    /// Gets or sets the experience rate.
    /// </summary>
    [ConfigurationKeyName("experience")]
    public int Experience { get; set; }

    /// <summary>
    /// Gets or sets the drop rate.
    /// </summary>
    [ConfigurationKeyName("drop")]
    public int Drop { get; set; }
}
