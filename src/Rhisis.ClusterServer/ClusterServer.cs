using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Client;
using Rhisis.ClusterServer.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Resources.Loaders;
using Rhisis.Network;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;
using System;

namespace Rhisis.ClusterServer
{
    /// <summary>
    /// Cluster server.
    /// </summary>
    public class ClusterServer : NetServer<ClusterClient>, IClusterServer
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;
        private readonly ILogger<ClusterServer> _logger;
        private readonly IGameResources _gameResources;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRhisisDatabase _database;
        private readonly IRhisisCacheManager _rhisisCacheManager;

        /// <inheritdoc />
        public ClusterConfiguration ClusterConfiguration { get; }

        /// <inheritdoc />
        public CoreConfiguration CoreConfiguration { get; }

        /// <summary>
        /// Creates a new <see cref="ClusterServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="clusterConfiguration">Cluster Server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public ClusterServer(ILogger<ClusterServer> logger, IOptions<ClusterConfiguration> clusterConfiguration, IOptions<CoreConfiguration> coreConfiguration, IGameResources gameResources, IServiceProvider serviceProvider, IRhisisDatabase database, IRhisisCacheManager rhisisCacheManager)
        {
            _logger = logger;
            ClusterConfiguration = clusterConfiguration.Value;
            CoreConfiguration = coreConfiguration.Value;
            _gameResources = gameResources;
            _serviceProvider = serviceProvider;
            _database = database;
            _rhisisCacheManager = rhisisCacheManager;
            PacketProcessor = new FlyffPacketProcessor();
            ServerConfiguration = new NetServerConfiguration("0.0.0.0",
                ClusterConfiguration.Port,
                ClientBacklog,
                ClientBufferSize);
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
            _logger.LogInformation($"'{ClusterConfiguration.Name}' cluster server is started and listening on {ServerConfiguration.Host}:{ServerConfiguration.Port}.");
        }

        protected override void OnClientConnected(ClusterClient client)
        {
            _logger.LogInformation($"New client connected to {nameof(ClusterServer)} from {client.Socket.RemoteEndPoint}.");

            client.Initialize(this,
                _serviceProvider.GetRequiredService<ILogger<ClusterClient>>(),
                _serviceProvider.GetRequiredService<IHandlerInvoker>());

            var clusterPacketFactory =  _serviceProvider.GetRequiredService<IClusterPacketFactory>();
            clusterPacketFactory.SendWelcome(client);
        }

        protected override void OnClientDisconnected(ClusterClient client)
        {
            _logger.LogInformation($"Client disconnected from {client.Socket.RemoteEndPoint}.");
        }
    }
}
