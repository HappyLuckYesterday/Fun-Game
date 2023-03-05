using LiteNetwork.Client;
using System;

namespace Rhisis.WorldServer;

public class WorldChannelCacheUser : LiteClient
{
    public WorldChannelCacheUser(LiteClientOptions options, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
    }
}
