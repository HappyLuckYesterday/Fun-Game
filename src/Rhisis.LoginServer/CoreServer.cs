using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer
{
    public class CoreServer : LiteServer<CoreUser>, ICoreServer
    {
        private readonly ILogger<CoreServer> _logger;

        public IEnumerable<Cluster> Clusters => ConnectedUsers.Select(x => x.Cluster);

        public CoreServer(LiteServerOptions serverOptions, ILogger<CoreServer> logger, IServiceProvider serviceProvider)
            : base(serverOptions, serviceProvider)
        {
            _logger = logger;
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"CoreServer started and listening on port '{Options.Port}'.");
        }

        public void SendToClusters(CorePacket packet)
        {
            SendTo(ConnectedUsers, packet.GetBuffer());
        }
    }
}
