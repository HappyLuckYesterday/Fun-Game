using Rhisis.Abstractions.Server;

namespace Rhisis.ClusterServer.Abstractions;

public interface ICoreClient
{
    void UpdateWorldChannel(WorldChannel channel);

    void RemoveWorldChannel(WorldChannel channel);
}