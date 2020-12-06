using Rhisis.LoginServer.Client;
using Rhisis.Network;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.LoginServer.Handlers
{
    [Handler]
    public class ErrorHandler
    {
        /// <summary>
        /// Disconnects the client when it receives an error packet.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="_"></param>
        [HandlerAction(PacketType.ERROR)]
        public void Execute(ILoginClient client, INetPacketStream _)
        {
            client.Disconnect();
        }
    }
}