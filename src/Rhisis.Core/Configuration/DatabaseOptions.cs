using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

/// <summary>
/// Provides options to configuration the database access.
/// </summary>
public sealed class DatabaseOptions
{
    /// <summary>
    /// Gets or sets the database provider.
    /// </summary>
    [ConfigurationKeyName("provider")]
    public DatabaseProviders Provider { get; set; }

    /// <summary>
    /// Gets or sets the database connection string.
    /// </summary>
    [ConfigurationKeyName("connection-string")]
    public string ConnectionString { get; set; }
}
