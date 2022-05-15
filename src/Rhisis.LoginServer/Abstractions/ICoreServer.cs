using Rhisis.Abstractions.Server;
using Rhisis.Protocol.Core;
using System.Collections.Generic;

namespace Rhisis.LoginServer.Abstractions
{
    /// <summary>
    /// Provides an abstraction that represents the core server.
    /// </summary>
    public interface ICoreServer
    {
        /// <summary>
        /// Gets the connected cluster list.
        /// </summary>
        IEnumerable<Cluster> Clusters { get; }

        /// <summary>
        /// Sends a packet to every connected clusters.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        void SendToClusters(CorePacket packet);
    }
}
