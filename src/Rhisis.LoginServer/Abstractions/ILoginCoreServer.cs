using LiteNetwork.Protocol.Abstractions;
using Rhisis.Protocol.Core.Servers;
using System.Collections.Generic;

namespace Rhisis.LoginServer.Abstractions
{
    /// <summary>
    /// Provides an abstraction that represents the core server.
    /// </summary>
    public interface ILoginCoreServer
    {
        /// <summary>
        /// Gets the connected cluster list.
        /// </summary>
        IEnumerable<Cluster> Clusters { get; }

        /// <summary>
        /// Sends a packet to every connected clusters.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        void SendToClusters(ILitePacketStream packet);
    }
}
