using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol.Core;
using Rhisis.Protocol.Core.Servers;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer
{
    public class LoginCoreServer : LiteServer<LoginCoreUser>, ILoginCoreServer
    {
        private readonly ILogger<LoginCoreServer> _logger;

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

        public LoginCoreServer(LiteServerOptions serverOptions, ILogger<LoginCoreServer> logger)
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
            IEnumerable<LoginCoreUser> clusters = ConnectedUsers.Where(x => x.ServerType == ServerType.Cluster);

            SendTo(clusters, packet);
        }
    }
}
