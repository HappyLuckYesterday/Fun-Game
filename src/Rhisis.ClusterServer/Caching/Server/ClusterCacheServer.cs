using LiteNetwork.Server;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.ClusterServer.Caching.Server;

public class ClusterCacheServer : LiteServer<ClusterCacheUser>
{
    public ClusterCacheServer(LiteServerOptions options, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
    }
}
