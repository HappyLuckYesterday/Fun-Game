using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

/// <summary>
/// Provides options to configure the login server.
/// </summary>
public sealed class LoginServerOptions
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
    /// Gets or sets the client build version.
    /// </summary>
    /// <remarks>
    /// During the authentication process, if this build version doesn't match the client build version
    /// you will not be allowed to connect to the Login Server.
    /// </remarks>
    [ConfigurationKeyName("build-version")]
    public string BuildVersion { get; set; }

    /// <summary>
    /// Gets or sets the value if we check the account verification.
    /// </summary>
    [ConfigurationKeyName("account-verification")]
    public bool AccountVerification { get; set; }

    /// <summary>
    /// Gets or sets the password encryption key.
    /// </summary>
    [ConfigurationKeyName("password-encryption-key")]
    public string PasswordEncryptionKey { get; set; }
}
