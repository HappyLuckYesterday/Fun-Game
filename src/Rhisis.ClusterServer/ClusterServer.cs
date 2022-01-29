using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Core;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Resources;
using Rhisis.Game.Resources.Loaders;
using Rhisis.Infrastructure.Persistance;
using System;
using System.Linq;

namespace Rhisis.ClusterServer
{
    /// <summary>
    /// Cluster server.
    /// </summary>
    public class ClusterServer : LiteServer<ClusterUser>, IClusterServer
    {
        private readonly ILogger<ClusterServer> _logger;
        private readonly IOptions<ClusterConfiguration> _clusterConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IRhisisDatabase _database;
        private readonly IRhisisCacheManager _rhisisCacheManager;
        private readonly ClusterCoreClient _clusterCoreClient;

        /// <summary>
        /// Creates a new <see cref="ClusterServer"/> instance.
        /// </summary>
        /// <param name="options">Server options.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="clusterConfiguration">Cluster Server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="database">Database access.</param>
        /// <param name="rhisisCacheManager">Cache manager.</param>
        /// <param name="clusterCoreClient">Cluster Core client.</param>
        public ClusterServer(LiteServerOptions options, 
            ILogger<ClusterServer> logger, 
            IOptions<ClusterConfiguration> clusterConfiguration, 
            IGameResources gameResources, 
            IRhisisDatabase database, 
            IRhisisCacheManager rhisisCacheManager,
            ClusterCoreClient clusterCoreClient)
            : base(options)
        {
            _logger = logger;
            _clusterConfiguration = clusterConfiguration;
            _gameResources = gameResources;
            _database = database;
            _rhisisCacheManager = rhisisCacheManager;
            _clusterCoreClient = clusterCoreClient;
        }

        protected override void OnBeforeStart()
        {
            try
            {
                _rhisisCacheManager.ClearAllCaches();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Failed to clear cluster cache.");
            }

            if (!_database.IsAlive())
            {
                throw new InvalidProgramException($"Cannot start {nameof(ClusterServer)}. Failed to reach database.");
            }

            _gameResources.Load(typeof(DefineLoader), typeof(JobLoader));
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"'{_clusterConfiguration.Value.Name}' cluster server is started and listening on {Options.Host}:{Options.Port}.");
            _clusterCoreClient.ConnectAsync().Wait();
        }

        public IClusterUser GetClientByUserId(int userId)
        {
            return ConnectedUsers.FirstOrDefault(x => x.UserId == userId);
        }
    }
}
