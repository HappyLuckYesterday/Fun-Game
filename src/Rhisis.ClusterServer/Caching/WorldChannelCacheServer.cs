using LiteNetwork.Server;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.ClusterServer.Caching;

public class WorldChannelCacheServer : LiteServer<WorldChannelUser>, IWorldChannelCache
{
    public WorldChannelCacheServer(LiteServerOptions options, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
    }

    public IReadOnlyList<WorldChannelInfo> GetWorldChannels()
    {
        return Users.Cast<WorldChannelUser>().Where(x => x.WorldChannel is not null).Select(x => x.WorldChannel).ToList();
    }

    public WorldChannelInfo GetWorldChannel(string channelName)
    {
        return GetWorldChannels().SingleOrDefault(x => x.Name == channelName);
    }
}
