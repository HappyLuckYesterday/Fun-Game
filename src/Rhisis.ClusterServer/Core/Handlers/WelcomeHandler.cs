using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.ClusterServer.Core.Handlers
{
    [Handler]
    public class WelcomeHandler
    {
        private readonly IClusterServer _clusterServer;

        public WelcomeHandler(IClusterServer clusterServer)
        {
            _clusterServer = clusterServer;
        }

        [HandlerAction(CorePacketType.Welcome)]
        public void OnExecute(ClusterCoreClient client, INetPacketStream _)
        {
            using var packet = new NetPacket();

            packet.WriteByte(value: (byte)CorePacketType.Authenticate);
            packet.WriteString(_clusterServer.CoreConfiguration.Password);
            packet.WriteByte((byte)ServerType.Cluster);
            packet.WriteByte((byte)_clusterServer.ClusterConfiguration.Id);
            packet.WriteString(_clusterServer.ClusterConfiguration.Name);
            packet.WriteString(_clusterServer.ClusterConfiguration.Host);
            packet.WriteUInt16((ushort)_clusterServer.ClusterConfiguration.Port);

            client.Send(packet);
        }
    }
}
