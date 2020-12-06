using Rhisis.LoginServer.Client;
using Rhisis.LoginServer.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.LoginServer.Handlers
{
    [Handler]
    public class PingHandler
    {
        private readonly ILoginPacketFactory _loginPacketFactory;

        public PingHandler(ILoginPacketFactory loginPacketFactory)
        {
            _loginPacketFactory = loginPacketFactory;
        }

        /// <summary>
        /// Handles the PING packet.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="pingPacket">Ping packet.</param>
        [HandlerAction(PacketType.PING)]
        public void OnPing(ILoginClient client, PingPacket pingPacket)
        {
            if (!pingPacket.IsTimeOut)
            {
                _loginPacketFactory.SendPong(client, pingPacket.Time);
            }
        }
    }
}