using Rhisis.ClusterServer.Client;
using Rhisis.Core.Structures.Configuration;
using Sylver.Network.Server;

namespace Rhisis.ClusterServer
{
    /// <summary>
    /// Provides a mechanism to manage the cluster server instance.
    /// </summary>
    public interface IClusterServer : INetServer
    {
        /// <summary>
        /// Gets the cluster server's configuration.
        /// </summary>
        ClusterConfiguration ClusterConfiguration { get; }

        /// <summary>
        /// Gets the cluster server's core configuration.
        /// </summary>
        CoreConfiguration CoreConfiguration { get; }

        /// <summary>
        /// Gets a cluster client by its user id.
        /// </summary>
        /// <param name="userId">Client user id.</param>
        /// <returns></returns>
        IClusterClient GetClientByUserId(int userId);
    }
}