using LiteNetwork;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Abstractions.Resources;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Game.Resources.Loaders;
using Rhisis.Infrastructure.Persistance;
using System;
using System.Linq;

namespace Rhisis.ClusterServer;

/// <summary>
/// Cluster server.
/// </summary>
public class ClusterServer : LiteServer<ClusterUser>, IClusterServer
{
    private readonly ILogger<ClusterServer> _logger;
    private readonly IOptions<ClusterOptions> _clusterConfiguration;
    private readonly IGameResources _gameResources;
    private readonly IRhisisDatabase _database;

    /// <summary>
    /// Creates a new <see cref="ClusterServer"/> instance.
    /// </summary>
    /// <param name="options">Server options.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="clusterConfiguration">Cluster Server configuration.</param>
    /// <param name="gameResources">Game resources.</param>
    /// <param name="database">Database access.</param>
    /// <param name="serviceProvider">Service provider.</param>
    public ClusterServer(LiteServerOptions options,
        ILogger<ClusterServer> logger,
        IOptions<ClusterOptions> clusterConfiguration,
        IGameResources gameResources,
        IRhisisDatabase database,
        IServiceProvider serviceProvider)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _clusterConfiguration = clusterConfiguration;
        _gameResources = gameResources;
        _database = database;
    }

    protected override void OnBeforeStart()
    {
        if (!_database.IsAlive())
        {
            throw new InvalidProgramException($"Cannot start {nameof(ClusterServer)}. Failed to reach database.");
        }

        _gameResources.Load(typeof(DefineLoader), typeof(JobLoader));
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"'{_clusterConfiguration.Value.Name}' cluster server is started and listening on {Options.Host}:{Options.Port}.");
    }

    protected override void OnError(LiteConnection connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(ClusterServer).Name}.");
    }

    public IClusterUser GetClientByUserId(int userId)
    {
        return Users.Cast<ClusterUser>().FirstOrDefault(x => x.UserId == userId);
    }
}
