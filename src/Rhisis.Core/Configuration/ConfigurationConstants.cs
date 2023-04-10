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
    /// Prevents from creating a <see cref="ConfigurationConstants"/> instance from outside.
    /// </summary>
    private ConfigurationConstants()
    {
    }
}
