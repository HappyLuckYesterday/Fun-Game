using Microsoft.Extensions.Logging;
using Rhisis.Protocol;

namespace Rhisis.ClusterServer;

public sealed class ClusterUser : FFUserConnection
{
    public ClusterUser(ILogger<ClusterUser> logger) 
        : base(logger)
    {
    }
}
