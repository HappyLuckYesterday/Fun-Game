using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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

    /// <summary>
    /// Gets or sets the available maps on the cluster.
    /// </summary>
    [ConfigurationKeyName("maps")]
    public IEnumerable<string> Maps { get; set; }

    /// <summary>
    /// Gets or sets the rates.
    /// </summary>
    [ConfigurationKeyName("rates")]
    public RateOptions Rates { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the death penality system is enabled.
    /// </summary>
    [ConfigurationKeyName("death-penality-enabled")]
    public bool DeathPenalityEnabled { get; set; }

    /// <summary>
    /// Gets or sets the mail configuration.
    /// </summary>
    [ConfigurationKeyName("mails")]
    public MailOptions Mails { get; set; }

    /// <summary>
    /// Gets or sets the messenger configuration.
    /// </summary>
    [ConfigurationKeyName("messenger")]
    public MessengerOptions Messenger { get; set; }

    /// <summary>
    /// Gets or sets the customization configuration.
    /// </summary>
    [ConfigurationKeyName("customization")]
    public CustomizationOptions Customization { get; set; }

    /// <summary>
    /// Gets or sets the drops configuration.
    /// </summary>
    [ConfigurationKeyName("drops")]
    public DropOptions Drops { get; set; }
}
