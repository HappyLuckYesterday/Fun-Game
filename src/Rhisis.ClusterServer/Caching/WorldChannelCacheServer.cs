using LiteNetwork.Server;
using System;

namespace Rhisis.ClusterServer.Caching;

public class WorldChannelCacheServer : LiteServer<WorldChannelUser>
{
    public WorldChannelCacheServer(LiteServerOptions options, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
    }
}
