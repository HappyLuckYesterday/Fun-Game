using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration.Cluster;

/// <summary>
/// Provides options to configure the cluster server.
/// </summary>
public sealed class ClusterServerOptions
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
    /// Gets or sets a boolean value that indicates if the login protect system is activated.
    /// </summary>
    [ConfigurationKeyName("login-protect")]
    public bool LoginProtectEnabled { get; set; }

    /// <summary>
    /// Gets or sets the default character configuration options.
    /// </summary>
    [ConfigurationKeyName("default-character")]
    public DefaultCharacterSection DefaultCharacter { get; set; }
}
