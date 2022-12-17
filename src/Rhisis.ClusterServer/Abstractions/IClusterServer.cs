using System;

namespace Rhisis.ClusterServer.Abstractions;

/// <summary>
/// Provides a mechanism to manage the cluster server instance.
/// </summary>
public interface IClusterServer
{
    /// <summary>
    /// Gets a cluster client by its user id.
    /// </summary>
    /// <param name="userId">Client user id.</param>
    /// <returns></returns>
    IClusterUser GetClientByUserId(int userId);

    /// <summary>
    /// Disconnects the user identified by the user id.
    /// </summary>
    /// <param name="userId">User id to disconnect.</param>
    void DisconnectUser(Guid userId);
}