using Rhisis.Network.Core.Servers;
using System.Collections.Generic;

namespace Rhisis.Network.Protocol.Messages
{
    /// <summary>
    /// Message to update the servers list.
    /// </summary>
    public class ServerListUpdateMessage
    {
        /// <summary>
        /// Gets or sets ta collection of connected clusters.
        /// </summary>
        public IEnumerable<Cluster> Clusters { get; set; }
    }
}
