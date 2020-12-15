using Rhisis.ClusterServer.Client;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;

namespace Rhisis.ClusterServer.Core.Handlers
{
    [Handler]
    public class DisconnectClientFromClusterHandler
    {
        private readonly IClusterServer _clusterServer;

        public DisconnectClientFromClusterHandler(IClusterServer clusterServer)
        {
            _clusterServer = clusterServer;
        }

        [HandlerAction(CorePacketType.DisconnectUserFromCluster)]
        public void OnExecute(ClusterCoreClient client, INetPacketStream packet)
        {
            int userId = packet.ReadInt32();
            IClusterClient clusterClient = _clusterServer.GetClientByUserId(userId);

            if (clusterClient is null)
            {
                throw new InvalidOperationException($"Cannot find cluster client with id: '{userId}'.");
            }

            clusterClient.Disconnect();
        }
    }
}
