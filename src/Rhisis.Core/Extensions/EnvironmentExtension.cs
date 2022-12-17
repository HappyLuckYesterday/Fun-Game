﻿using Rhisis.Core.Structures.Configuration;
using System;

namespace Rhisis.Core.Extensions;

public static class EnvironmentExtension
{
    /// <summary>
    /// Checks if the program is running inside a docker container.
    /// </summary>
    /// <returns>True if running inside docker; false otherwise.</returns>
    public static bool IsRunningInDocker()
    {
        return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    }

    public static string GetCurrentEnvironementDirectory()
    {
        if (IsRunningInDocker())
        {
            return Environment.GetEnvironmentVariable(ConfigurationConstants.RhisisDockerConfigurationKey)
                                        ?? ConfigurationConstants.DefaultRhisisDockerConfigurationPath;
        }

        return Environment.CurrentDirectory;
    }
}
