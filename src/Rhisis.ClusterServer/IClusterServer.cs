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
    }
}