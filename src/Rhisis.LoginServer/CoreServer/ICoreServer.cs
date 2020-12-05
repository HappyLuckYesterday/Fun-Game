using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core.Servers;
using Sylver.Network.Server;
using System.Collections.Generic;

namespace Rhisis.LoginServer.CoreServer
{
    public interface ICoreServer : INetServer
    {
        IEnumerable<Cluster> Clusters { get; }

        CoreConfiguration Configuration { get; }
    }
}
