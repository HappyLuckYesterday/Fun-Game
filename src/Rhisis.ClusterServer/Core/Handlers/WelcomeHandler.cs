using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.ClusterServer.Core.Handlers
{
    [Handler]
    public class WelcomeHandler
    {
        private readonly IOptions<ClusterOptions> _clusterOptions;

        public WelcomeHandler(IOptions<ClusterOptions> clusterOptions)
        {
            _clusterOptions = clusterOptions;
        }

        [HandlerAction(CorePacketType.Welcome)]
        public void OnExecute(ClusterCoreClient client, CorePacket _)
        {
            using var packet = new CorePacket();

            packet.WriteByte((byte)CorePacketType.AuthenticationRequest);
            packet.WriteString(_clusterOptions.Value.Core.Password);
            packet.WriteInt32(_clusterOptions.Value.Id);
            packet.WriteString(_clusterOptions.Value.Name);
            packet.WriteString(_clusterOptions.Value.Host);
            packet.WriteUInt16((ushort)_clusterOptions.Value.Port);

            // TODO: serialize world servers
            packet.WriteByte(0); // world channel count

            client.Send(packet);
        }
    }
}
