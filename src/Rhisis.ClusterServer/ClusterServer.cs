using LiteNetwork.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Infrastructure.Persistance;
using System;

namespace Rhisis.ClusterServer;

public sealed class ClusterServer : LiteServer<ClusterUser>
{
    private readonly ILogger<ClusterServer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ClusterServer(LiteServerOptions options, ILogger<ClusterServer> logger, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override void OnBeforeStart()
    {
        if (_serviceProvider is not null)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IAccountDatabase accountDatabase = scope.ServiceProvider.GetService<IAccountDatabase>();
            IGameDatabase gameDatabase = scope.ServiceProvider.GetService<IGameDatabase>();

            accountDatabase.Migrate();
            gameDatabase.Migrate();
        }

        base.OnBeforeStart();
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Login Server listening on port {Options.Port}.");
    }

    protected override void OnError(ClusterUser connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(ClusterServer).Name}.");
    }
}
