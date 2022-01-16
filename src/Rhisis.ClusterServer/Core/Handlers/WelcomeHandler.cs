using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Core.Handlers
{
    [Handler]
    public class WelcomeHandler
    {
        private readonly IOptions<ClusterConfiguration> _clusterOptions;
        private readonly IOptions<CoreConfiguration> _coreOptions;

        public WelcomeHandler(IOptions<ClusterConfiguration> clusterOptions, IOptions<CoreConfiguration> coreOptions)
        {
            _clusterOptions = clusterOptions;
            _coreOptions = coreOptions;
        }

        [HandlerAction(LoginCorePacketType.Welcome)]
        public void OnExecute(ClusterCoreClient client, ILitePacketStream _)
        {
            using var packet = new LitePacket();

            packet.WriteByte(value: (byte)LoginCorePacketType.Authenticate);
            packet.WriteString(_coreOptions.Value.Password);
            packet.WriteByte((byte)ServerType.Cluster);
            packet.WriteByte((byte)_clusterOptions.Value.Id);
            packet.WriteString(_clusterOptions.Value.Name);
            packet.WriteString(_clusterOptions.Value.Host);
            packet.WriteUInt16((ushort)_clusterOptions.Value.Port);

            client.Send(packet);
        }
    }
}
