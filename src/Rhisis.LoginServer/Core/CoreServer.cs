using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.LoginServer.Core.Abstractions;
using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer.Core
{
    public class CoreServer : LiteServer<CoreServerClient>, ICoreServer
    {
        private readonly ILogger<CoreServer> _logger;

        public IEnumerable<Cluster> Clusters
        {
            get
            {
                var clusters = ConnectedUsers.Where(x => x.ServerType is ServerType.Cluster).Select(x => new Cluster
                {
                    Id = x.ServerInfo.Id,
                    Name = x.ServerInfo.Name,
                    Host = x.ServerInfo.Host,
                    Port = x.ServerInfo.Port,
                    Channels = ConnectedUsers.Where(w => w.ServerType is ServerType.World && (w.ServerInfo as WorldChannel).ClusterId == x.ServerInfo.Id)
                        .Select(x => x.ServerInfo as WorldChannel)
                        .ToList()
                });

                return clusters;
            }
        }

        public CoreServer(LiteServerOptions serverOptions, ILogger<CoreServer> logger)
            : base(serverOptions)
        {
            _logger = logger;
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"CoreServer started and listening on port '{Options.Port}'.");
        }

        public void SendToClusters(ILitePacketStream packet)
        {
            IEnumerable<CoreServerClient> clusters = ConnectedUsers.Where(x => x.ServerType == ServerType.Cluster);

            SendTo(clusters, packet);
        }
    }
}
