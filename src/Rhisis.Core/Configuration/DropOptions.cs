using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class DropOptions
{
    /// <summary>
    /// Gets or sets the drop ownership time in seconds.
    /// </summary>
    [ConfigurationKeyName("ownership-time")]
    public long OwnershipTime { get; set; }

    /// <summary>
    /// Gets or sets the drop despawn time in seconds.
    /// </summary>
    [ConfigurationKeyName("despawn-time")]
    public long DespawnTime { get; set; }
}
