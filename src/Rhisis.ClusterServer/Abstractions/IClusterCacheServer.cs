using Rhisis.Abstractions.Server;
using System.Collections.Generic;

namespace Rhisis.ClusterServer.Abstractions;

public interface IClusterCacheServer
{
    IEnumerable<WorldChannel> WorldChannels { get; }

    void SendToAll(byte[] packet);
}