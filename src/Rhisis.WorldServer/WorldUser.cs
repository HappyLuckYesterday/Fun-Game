using Microsoft.Extensions.Logging;
using Rhisis.Protocol;

namespace Rhisis.WorldServer;

public sealed class WorldUser : FFUserConnection
{
    public WorldUser(ILogger<WorldUser> logger) 
        : base(logger)
    {
    }
}