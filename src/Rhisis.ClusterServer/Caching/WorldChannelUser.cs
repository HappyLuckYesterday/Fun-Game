using LiteNetwork.Server;
using Rhisis.Protocol;

namespace Rhisis.ClusterServer.Caching;

public class WorldChannelUser : LiteServerUser
{
    public WorldChannelInfo WorldChannel { get; set; }
}
