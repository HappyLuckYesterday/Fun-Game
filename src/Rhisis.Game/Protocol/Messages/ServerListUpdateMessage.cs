using Rhisis.Network.Core.Servers;
using System.Collections.Generic;

namespace Rhisis.Game.Protocol.Messages
{
    public class NewClusterMessage
    {
        public int ClusterId { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }

    public class RemoveClusterMessage
    {
        public int ClusterId { get; set; }
    }
}
