using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.ClusterServer.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.ClusterServer.Cache
{
    internal class ClusterCacheServer : LiteServer<ClusterCacheUser>, IClusterCacheServer
    {
        private readonly ILogger<ClusterCacheServer> _logger;

        public IEnumerable<WorldChannel> WorldChannels => ConnectedUsers.Select(x => x.Channel);

        public ClusterCacheServer(LiteServerOptions options, ILogger<ClusterCacheServer> logger) 
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"Cluster cache server started and listening on port '{Options.Port}'.");
        }
    }
}
