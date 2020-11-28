using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.LoginServer.Client;
using Rhisis.LoginServer.Packets;
using Rhisis.Messaging.Abstractions;
using Rhisis.Network;
using Rhisis.Network.Core.Servers;
using Rhisis.Network.Protocol.Messages;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer
{
    public sealed class LoginServer : NetServer<LoginClient>, ILoginServer
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;
        private readonly ILogger<LoginServer> _logger;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRhisisDatabase _database;
        private readonly IMessaging _messaging;

        private Dictionary<int, Cluster> _clusters;

        public IReadOnlyDictionary<int, Cluster> ConnectedClusters => _clusters;

        /// <summary>
        /// Creates a new <see cref="LoginServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="loginConfiguration">Login server configuration.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public LoginServer(ILogger<LoginServer> logger, IOptions<LoginConfiguration> loginConfiguration, 
            IServiceProvider serviceProvider, IRhisisDatabase database, IMessaging messaging)
        {
            _logger = logger;
            _loginConfiguration = loginConfiguration.Value;
            _serviceProvider = serviceProvider;
            _database = database;
            _messaging = messaging;
            _clusters = new Dictionary<int, Cluster>();
            PacketProcessor = new FlyffPacketProcessor();
            ServerConfiguration = new NetServerConfiguration("0.0.0.0", 
                _loginConfiguration.Port, 
                ClientBacklog, 
                ClientBufferSize);
        }

        /// <inheritdoc />
        protected override void OnBeforeStart()
        {
            if (!_database.IsAlive())
            {
                throw new InvalidProgramException($"Cannot start {nameof(LoginServer)}. Failed to reach database.");
            }

            _messaging.Subscribe<ServerListUpdateMessage>(message => OnServerListUpdate(message));
        }

        /// <inheritdoc />
        protected override void OnAfterStart()
        {
            //TODO: Implement this log inside OnStarted method when will be available.
            _logger.LogInformation($"{nameof(LoginServer)} is started and listen on {ServerConfiguration.Host}:{ServerConfiguration.Port}.");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(LoginClient client)
        {
            _logger.LogInformation($"New client connected to {nameof(LoginServer)} from {client.Socket.RemoteEndPoint}.");

            client.Initialize(this, 
                _serviceProvider.GetRequiredService<ILogger<LoginClient>>(), 
                _serviceProvider.GetRequiredService<IHandlerInvoker>(),
                _serviceProvider.GetRequiredService<ILoginPacketFactory>());
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(LoginClient client)
        {
            if (string.IsNullOrEmpty(client.Username))
                _logger.LogInformation($"Unknown client disconnected from {client.Socket.RemoteEndPoint}.");
            else
                _logger.LogInformation($"Client '{client.Username}' disconnected from {client.Socket.RemoteEndPoint}.");
        }

        /// <inheritdoc />
        public ILoginClient GetClientByUsername(string username)
            => Clients.FirstOrDefault(x =>
                x.IsConnected &&
                x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        /// <inheritdoc />
        public bool IsClientConnected(string username) => GetClientByUsername(username) != null;

        private void OnServerListUpdate(ServerListUpdateMessage message)
        {
            lock (_clusters)
            {
                _clusters = message.Clusters.ToDictionary(x => x.Id, x => x);
                _logger.LogTrace($"Cluster server list updated. Available clusters: {_clusters.Count}");
            }
        }
    }
}