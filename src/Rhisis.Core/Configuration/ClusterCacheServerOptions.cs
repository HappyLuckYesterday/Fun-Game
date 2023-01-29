using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class ClusterCacheServerOptions
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
    /// Gets or sets the server's master password.
    /// </summary>
    [ConfigurationKeyName("master-password")]
    public string MasterPassword { get; set; }
}
