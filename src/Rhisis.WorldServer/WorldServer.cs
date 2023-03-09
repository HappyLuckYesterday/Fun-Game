using LiteNetwork.Server;
using Rhisis.WorldServer.Abstractions;
using System;

namespace Rhisis.WorldServer;

public sealed class WorldServer : LiteServer<WorldUser>, IWorldChannel
{
    public string Name => throw new NotImplementedException();

    public string Ip => throw new NotImplementedException();

    public int Port => throw new NotImplementedException();

    public string Cluster => throw new NotImplementedException();

    public WorldServer(LiteServerOptions options, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
    }
}
