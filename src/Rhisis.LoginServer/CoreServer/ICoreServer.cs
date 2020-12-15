using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core.Servers;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System.Collections.Generic;

namespace Rhisis.LoginServer.CoreServer
{
    /// <summary>
    /// Provides an abstraction that represents the core server.
    /// </summary>
    public interface ICoreServer : INetServer
    {
        /// <summary>
        /// Gets the connected cluster list.
        /// </summary>
        IEnumerable<Cluster> Clusters { get; }

        /// <summary>
        /// Gets the core server configuration.
        /// </summary>
        CoreConfiguration Configuration { get; }

        /// <summary>
        /// Sends a packet to every connected clusters.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        void SendToClusters(INetPacketStream packet);
    }
}
