using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class ClusterCacheClientOptions : CoreCacheBaseOptions
{
    /// <summary>
    /// Gets or sets the cluster name.
    /// </summary>
    [ConfigurationKeyName("name")]
    public string Name { get; set; }
}