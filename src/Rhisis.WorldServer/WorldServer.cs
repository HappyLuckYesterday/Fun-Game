using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.WorldServer.Abstractions;
using System;

namespace Rhisis.WorldServer;

public sealed class WorldServer : LiteServer<WorldUser>, IWorldChannel
{
    private readonly ILogger<WorldServer> _logger;

    public string Name => throw new NotImplementedException();

    public string Ip => throw new NotImplementedException();

    public int Port => throw new NotImplementedException();

    public string Cluster => throw new NotImplementedException();

    public WorldServer(LiteServerOptions options, ILogger<WorldServer> logger, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"World Server listening on port {Options.Port}.");
    }

    protected override void OnError(WorldUser connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(WorldServer).Name}.");
    }
}
