using Microsoft.Extensions.Configuration;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Rhisis.Core.Configuration;

public sealed class WorldChannelServerOptions
{
    /// <summary>
    /// Gets or sets the server's listening IP address.
    /// </summary>
    /// <remarks>
    /// When null or empty, the server listen on all network interfaces.
    /// </remarks>
    [ConfigurationKeyName("ip")]
    public string Ip { get; set; }

    /// <summary>
    /// Gets or sets the server's listening port.
    /// </summary>
    [ConfigurationKeyName("port")]
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the server's name.
    /// </summary>
    [ConfigurationKeyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the server's parent cluster name.
    /// </summary>
    [ConfigurationKeyName("cluster")]
    public ClusterCacheClientOptions Cluster { get; set; }

    /// <summary>
    /// Gets or sets the server's maximum allowed users.
    /// </summary>
    [ConfigurationKeyName("maximum-users")]
    public int MaximumUsers { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the channel allows player killing (PK).
    /// </summary>
    [ConfigurationKeyName("pk-enabled")]
    public bool IsPlayerKillingEnabled { get; set; }
}
