namespace Rhisis.Core.Configuration;

/// <summary>
/// Contains contants related to the configuration.
/// </summary>
public sealed class ConfigurationConstants
{
    /// <summary>
    /// Gets the default configuration path when Rhisis is running inside a container.
    /// </summary>
    public static readonly string DefaultRhisisDockerConfigurationPath = "/var/rhisis";

    /// <summary>
    /// Gets the Rhisis docker configuration environment key.
    /// </summary>
    public static readonly string RhisisDockerConfigurationKey = "RHISIS_CONFIGURATION";

    /// <summary>
    /// Gets the CoreServer configuration path.
    /// </summary>
    public static readonly string CoreServerPath = "./config/core.yaml";

    /// <summary>
    /// Gets the LoginServer configuration file path.
    /// </summary>
    public static readonly string LoginServerPath = "./config/login.yaml";

    /// <summary>
    /// Gets the ClusterServer configuration file path.
    /// </summary>
    public static readonly string ClusterServerPath = "./config/cluster.yaml";

    /// <summary>
    /// Gets the WorldClusterServer configuration key.
    /// </summary>
    public static readonly string WorldClusterServer = "worldClusterServer";

    /// <summary>
    /// Gets the WorldServer configuration file path.
    /// </summary>
    public static readonly string WorldServerPath = "./config/world.yaml";

    /// <summary>
    /// Gets the Database configuration file path.
    /// </summary>
    public static readonly string DatabasePath = "config/database.yaml";

    /// <summary>
    /// Gets the server configuration key.
    /// </summary>
    public static readonly string ServerConfigurationKey = "server";

    /// <summary>
    /// Gets the Database configuration key.
    /// </summary>
    public static readonly string DatabaseConfiguration = "databaseConfiguration";

    /// <summary>
    /// Prevents from creating a <see cref="ConfigurationConstants"/> instance from outside.
    /// </summary>
    private ConfigurationConstants()
    {
    }
}
