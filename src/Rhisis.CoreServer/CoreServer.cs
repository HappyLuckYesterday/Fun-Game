using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Messaging.Abstractions;
using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using Rhisis.Network.Protocol.Messages;
using Sylver.Network.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.CoreServer
{
    public class CoreServer : NetServer<CoreServerClient>, ICoreServer
    {
        private readonly ILogger<CoreServer> _logger;
        private readonly IMessaging _messaging;
        private readonly IServiceProvider _serviceProvider;

        public IEnumerable<Cluster> Clusters
        {
            get
            {
                var clusters = Clients.Where(x => x.ServerType is ServerType.Cluster).Select(x => new Cluster
                {
                    Id = x.ServerInfo.Id,
                    Name = x.ServerInfo.Name,
                    Host = x.ServerInfo.Host,
                    Port = x.ServerInfo.Port,
                    Channels = Clients.Where(w => w.ServerType is ServerType.World && (w.ServerInfo as WorldChannel).ClusterId == x.ServerInfo.Id)
                        .Select(x => x.ServerInfo as WorldChannel)
                        .ToList()
                });

                return clusters;
            }
        }

        public CoreConfiguration Configuration { get; }

        public CoreServer(ILogger<CoreServer> logger, IOptions<CoreConfiguration> configuration, IMessaging messaging, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _messaging = messaging;
            _serviceProvider = serviceProvider;
            Configuration = configuration.Value;
            ServerConfiguration = new NetServerConfiguration(Configuration.Host, Configuration.Port, 50, 32);
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"CoreServer started and listening on port '{ServerConfiguration.Port}'.");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(CoreServerClient connection)
        {
            _logger.LogTrace($"New incoming server connection from {connection.Socket.RemoteEndPoint}.");

            connection.Initialize(_serviceProvider);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(CoreServerClient connection)
        {
            SendServerListUpdate();
        }

        public void SendServerListUpdate()
        {
            _messaging.Publish(new ServerListUpdateMessage
            {
                Clusters = Clusters
            });
        }
    }
}
