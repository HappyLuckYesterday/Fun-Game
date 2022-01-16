using LiteNetwork.Protocol.Abstractions;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;
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

        [HandlerAction(LoginCorePacketType.DisconnectUserFromCluster)]
        public void OnExecute(ClusterCoreClient _, ILitePacketStream packet)
        {
            int userId = packet.ReadInt32();
            IClusterUser clusterClient = _clusterServer.GetClientByUserId(userId);

            if (clusterClient is null)
            {
                throw new InvalidOperationException($"Cannot find cluster client with id: '{userId}'.");
            }

            clusterClient.Disconnect();
        }
    }
}
